using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using LuaInterface;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine.EventSystems;

[AddComponentMenu("UI/Custom/UIList")]
[RequireComponent(typeof(ScrollRect))]
public class UIList : MonoBehaviour, IBeginDragHandler
{
	public RectTransform itemPrefab;
    public RectTransform[] itemPrefabList;
	private LuaTable _listData;
	// Use this for initialization
	private RectTransform _viewPort;
	private RectTransform _content;
    private ScrollRect _scrollRect;
	private ListItemPool _itemPool;
    private Dictionary<string, ListItemPool> _poolDic;

	private List<RectTransform> _currentItems;	//正在显示的几个item的列表
	private float[] _heightCache;	//缓存每个item高度
	private float[] _yCache;	//缓存每个item的位置
	private int _startIndex;	//当前显示列表第一个item的index	
	private int _endIndex;		//当前显示列表最后一个item的index	
	private bool _isDataDirty;	//标记数据是否被修改，数据修改后将在下一个update循环中更新界面
    private int _dataLength;    //数据长度
    private Tween _tween;   //跳转时播放的动画

	public float gap = 0;				//两个item之间距离
	public float startGap = 0;			//第一个item距离viewport顶端距离
	public float endGap = 0;			//最后一个item距离viewport底端距离
	public bool isFixedSize = true;	//为true表示item等高等宽
    public float itemHeight = 50;
    public float itemWidth = 50;

    public LuaFunction CellSizeForTable;    //从lua指定每个item大小，当item高度不同时，可注册此函数
    public LuaFunction NumberOfCellsInTableView;    //从lua指定列表长度
    public LuaFunction PrefabAtIndex;   //从lua指定每个item使用的prefab

	void Awake ()
	{
        _scrollRect = gameObject.GetComponent<ScrollRect>();
        _viewPort = _scrollRect.viewport;
        _content = _scrollRect.content;

		_itemPool = new ListItemPool ();
        //if (_listData == null) {
			//this.ListData = new List<object>();
        //}

		Reload ();
	}

	void Start ()
	{
        _scrollRect.onValueChanged.AddListener(_OnScroll);
	}

	void OnDestroy()
	{
			
	}

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        _StopScrollAnim();
    }

	virtual protected void _Purge()
	{
		_removeAllItem ();

		_startIndex = 0;
		_endIndex = 0;
	}

	virtual protected void _removeAllItem()
	{
		if (_currentItems != null) {
			foreach (RectTransform item in _currentItems) {
				item.gameObject.SetActive (false);
                _EnqueueReusableItem(item);
			}
            _currentItems.Clear();
		}
        else {
            _currentItems = new List<RectTransform>();
        }
	}

	private void _OnReload()
	{
        _StopScrollAnim();
		_Purge ();
        this._dataLength = this._NumberOfCellsInTableView();
		_CalculateAllPosition ();
		_CalculateStartIndex ();
		_UpdateLayout ();
	}

		
		
	// Update is called once per frame
	void LateUpdate ()
	{
		if (_isDataDirty) {
			_isDataDirty = false;
			_OnReload ();
		}
	}


    private void _OnScroll(Vector2 arg0)
    {
		_UpdateLayout ();
	}

	// reload所有数据时，计算当前滚动位置对应的startindex
	private void _CalculateStartIndex()
	{
        if (_dataLength == 0)
        {
			return;
		}
        if (_scrollRect.vertical)
        {
            while (_startIndex < _yCache.Length && -_yCache[_startIndex] < _content.localPosition.y)
            {
                _startIndex++;
            }
        }
        else {
            while (_startIndex < _yCache.Length && _yCache[_startIndex] < -_content.localPosition.x)
            {
                this._startIndex++;
            }
        }
      
		_endIndex = _startIndex;
	}

    private void _UpdateLayoutHorizon()
    { 
        float maxY =  _viewPort.rect.width - _content.localPosition.x;
         //当第一个元素底端移动到viewport外时，删除之
        while (_startIndex < _dataLength - 1 && _yCache.Length > 0 && _yCache[_startIndex] + _heightCache[_startIndex] < -_content.localPosition.x)
        {
            if (_currentItems.Count > 0) {
                RectTransform firstItem = _currentItems[0];
                firstItem.gameObject.SetActive(false);
                _EnqueueReusableItem(firstItem);
                _currentItems.RemoveAt(0);
            }
            _startIndex++;
        }

         //当最后一个元素底端移动到viewport外时，删除之
        while (_endIndex > 0 && _yCache.Length > 0 && _yCache[_endIndex - 1] > maxY)
        {
            if (_currentItems.Count > 0)
            {
                RectTransform lastItem = _currentItems[_currentItems.Count - 1];
                lastItem.gameObject.SetActive(false);
                _EnqueueReusableItem(lastItem);
                _currentItems.RemoveAt(_currentItems.Count - 1);
            }
            _endIndex--;
        }

        //当最后一个元素底端没有达到viewport底端时，添加新元素
        while (_endIndex < _dataLength && _yCache[_endIndex] <= maxY)
        {
             //跳转的时候，有可能一下子所有元素由viewport下方一下子都挪动到了viewport上方，这种情况下不能添加
            if (!(_yCache.Length>0 && _yCache[_endIndex] + _heightCache[_endIndex] < -_content.localPosition.x)){
                 int index = _endIndex;
                RectTransform itemTransform = TableCellAtIndex(index + 1);
                _currentItems.Add(itemTransform);
                float y = _yCache[_endIndex];
                Vector2 size = itemTransform.sizeDelta;
                Vector2 pivot = itemTransform.pivot;
                itemTransform.localPosition = new Vector3(y + size.x * pivot.x, -size.y * pivot.y, 0f);
            }
            this._endIndex++;
        }   

        //当第一个元素顶端没有达到viewport顶端时，添加新元素
        while (_startIndex > 0 && _yCache[_startIndex] > -_content.localPosition.x)
        {
            this._startIndex--;
            if (_yCache.Length>0 && _startIndex > 1 && _yCache[_startIndex - 1]  > maxY){
                //跳转的时候，有可能一下子所有元素由viewport上方一下子都挪动到了viewport下方，这种情况下不能添加
                continue;
            }
            int index = _startIndex;
            RectTransform itemTransform = TableCellAtIndex(index + 1);
            _currentItems.Insert(0, itemTransform);
            float y = _yCache[_startIndex];
            Vector2 size = itemTransform.sizeDelta;
            Vector2 pivot = itemTransform.pivot;
            itemTransform.localPosition = new Vector3(y + size.x * pivot.x, -size.y * pivot.y, 0f);
        }
    }

	private void _UpdateLayoutVertical()
	{
		
		float maxY = _viewPort.rect.height + _content.localPosition.y;
        //当第一个元素底端移动到viewport外时，删除之
        //增加_startIndex < _dataLength - 1的判断，顶多删除到只有1个元素，不然list长度就为空，滑到边界时不会弹回了
        while (_startIndex < _dataLength - 1 && _yCache.Length > 0 && -_yCache[_startIndex] + _heightCache[_startIndex] < _content.localPosition.y)
        {
            if (_currentItems.Count > 0)
            {
                RectTransform firstItem = _currentItems[0];
                firstItem.gameObject.SetActive(false);
                _EnqueueReusableItem(firstItem);
                _currentItems.RemoveAt(0);
            }

            _startIndex++;
        }

        //当最后一个元素底端移动到viewport外时，删除之
        while (_endIndex > 0 && _yCache.Length > 0 && -_yCache[_endIndex - 1] > maxY)
        {
            if (_currentItems.Count > 0)
            {
                RectTransform lastItem = _currentItems[_currentItems.Count - 1];
                lastItem.gameObject.SetActive(false);
                _EnqueueReusableItem(lastItem);
                _currentItems.RemoveAt(_currentItems.Count - 1);
            }

            _endIndex--;
        }

        //当最后一个元素底端没有达到viewport底端时，添加新元素
		while (_endIndex < _dataLength && -_yCache[_endIndex] <= maxY)
		{
            //跳转的时候，有可能一下子所有元素由viewport下方一下子都挪动到了viewport上方，这种情况下不能添加
            if (!(_yCache.Length > 0 && -_yCache[_endIndex] + _heightCache[_endIndex] < _content.localPosition.y))
            {
                int index = _endIndex;
                RectTransform itemTransform = TableCellAtIndex(index + 1);
                _currentItems.Add(itemTransform);
                float y = _yCache[_endIndex];
                Vector2 size = itemTransform.sizeDelta;
                Vector2 pivot = itemTransform.pivot;
                itemTransform.localPosition = new Vector3(size.x * pivot.x, y - size.y*pivot.y, 0f);
            }
			_endIndex++;
		}


		//当第一个元素顶端没有达到viewport顶端时，添加新元素
        while (_startIndex > 0 && -_yCache[_startIndex] > _content.localPosition.y)
		{
			_startIndex--;
            if (_yCache.Length > 0 && -_yCache[_startIndex] > maxY)
            {
                //跳转的时候，有可能一下子所有元素由viewport上方一下子都挪动到了viewport下方，这种情况下不能添加
                continue;
            }
			int index = _startIndex;
            RectTransform itemTransform = TableCellAtIndex(index + 1);
			_currentItems.Insert (0, itemTransform);
			float y = _yCache[_startIndex];
            Vector2 size = itemTransform.sizeDelta;
            Vector2 pivot = itemTransform.pivot;
            itemTransform.localPosition = new Vector3(size.x * pivot.x, y - size.y * pivot.y, 0f);
		}
	}

    private void _UpdateLayout()
    {
        if (_scrollRect.vertical)
        {
            _UpdateLayoutVertical();
        }
        else {
            _UpdateLayoutHorizon();
        }
    }

    private void _CalcuelateAllPositionHorizon()
    {
        int length = _dataLength;
        _heightCache = new float[length];
        _yCache = new float[length];

        float x = this.startGap;

        Vector2 size = this._CellSizeForTable(_listData, 0);
        for (int i = 0; i < length; i++)
        {
            if (!this.isFixedSize)
            {
                size = this._CellSizeForTable(_listData, i);
            }
            float width = size.x;
            _heightCache[i] = width;
            _yCache[i] = x;
            x += width;
            if (i < length - 1)
            {
                x += this.gap;
            }
        }
        _content.sizeDelta = new Vector2(x + endGap, _content.sizeDelta.y);
    }

    private void _CalcuelateAllPositionVertical()
    {
        int length = _dataLength;
        _heightCache = new float[length];
        _yCache = new float[length];


        float y = -this.startGap;

        Vector2 size = this._CellSizeForTable(_listData, 0);
        for (int i = 0; i < length; i++)
        {
            if (!this.isFixedSize)
            {
                size = this._CellSizeForTable(_listData, i);
            }
            float height = size.y;
            _heightCache[i] = height;
            _yCache[i] = y;
            y -= height;
            if (i < length - 1)
            {
                y -= this.gap;
            }
        }
        _content.sizeDelta = new Vector2(_content.sizeDelta.x, -y + this.endGap);
    }

	/*
		* 虽然item可以复用，但是依然需要提前计算好每个item的高度和总高度，否则没法确定整个list滑动区域的高度
		* scrollbar长短无法绘制，也不利于直接跳转到某个item
		*/ 
	private void _CalculateAllPosition()
	{
        if (_scrollRect.vertical)
        {
            _CalcuelateAllPositionVertical();
        }
        else
        {
            _CalcuelateAllPositionHorizon();
        }
	}

 

    public int _NumberOfCellsInTableView(){

        if (NumberOfCellsInTableView != null)
        {
            NumberOfCellsInTableView.BeginPCall();
            NumberOfCellsInTableView.PCall();
            int length = (int)NumberOfCellsInTableView.CheckLong();
            NumberOfCellsInTableView.EndPCall();
            return length;
        }      	
        if (_listData == null) {
            return 0;
        }
        return _listData.Length;
    }

    public RectTransform TableCellAtIndex(int index){
        RectTransform itemTransform = DequeueReuseableItem(_listData[index], index);
        itemTransform.gameObject.SetActive(true);
        itemTransform.SetParent(_content, false);
        itemTransform.localScale = Vector3.one;

        ListItem item = itemTransform.GetComponent<ListItem>();
        if (item != null) {
            item.RefreshWithData(_listData, index);
        }
        
        return itemTransform;
    }

    private RectTransform _PrefabAtIndex(object listData, int index)
    {
        if (PrefabAtIndex != null)
        {
            PrefabAtIndex.BeginPCall();
            PrefabAtIndex.Push(listData);
            PrefabAtIndex.Push(index);
            PrefabAtIndex.PCall();
            RectTransform prefab = PrefabAtIndex.CheckVariant() as RectTransform;
            PrefabAtIndex.EndPCall();
            return prefab;
        }      	
        return itemPrefab;
    }

    virtual protected RectTransform DequeueReuseableItem(object data, int index)
    {
        RectTransform prefab = _PrefabAtIndex(_listData, index);
        if (prefab == itemPrefab)
        {
            //通常情况下都只有一个prefab
            return _itemPool.DequeueReusableItem(prefab);
        }
        if (_poolDic == null) {
            _poolDic = new Dictionary<string, ListItemPool>();
        }
        ListItemPool pool = null;
        string name = prefab.name;
        if (!_poolDic.TryGetValue(name, out pool))
        {
            pool = new ListItemPool();
            _poolDic[name] = pool;
        }
        return pool.DequeueReusableItem(prefab);
    }

    private void _EnqueueReusableItem(RectTransform item)
    {
        if (PrefabAtIndex == null || (itemPrefab!= null && item.name == itemPrefab.name))
        {
            //通常情况下都只有一个prefab
            _itemPool.EnqueueReusableItem(item);
            return;
        }
        string name = item.name;
        if (_poolDic == null)
        {
            _poolDic = new Dictionary<string, ListItemPool>();
        }
        ListItemPool pool = null;
        if (!_poolDic.TryGetValue(name, out pool))
        {
            pool = new ListItemPool();
            _poolDic[name] = pool;
        }
        pool.EnqueueReusableItem(item);
    }

    private Vector2 _CellSizeForTable(object listData, int index) {
        if (CellSizeForTable != null)
        {
            CellSizeForTable.BeginPCall();
            CellSizeForTable.Push(listData);
            CellSizeForTable.Push(index + 1);
            CellSizeForTable.PCall();
            Vector2 size = CellSizeForTable.CheckVector2();
            CellSizeForTable.EndPCall();
            return size;
        }      	
        return new Vector2(this.itemWidth, this.itemHeight);
    }

    /// <summary>
    /// 刷新list数据
    /// </summary>
    public void Reload()
    {
        _isDataDirty = true;
    }

    /// <summary>
    /// 设置list对应的数据
    /// set的时候会reload表格
    /// </summary>
    public LuaTable ListData
    {
        get
        {
            return _listData;
        }
        set
        {
            _listData = value;
            this.Reload();
        }
    }

    /// <summary>
    /// 跳转到list指定index对应的item
    /// 没有动画
    /// </summary>
    /// <param name="index">item的index</param>
    public void ScrollToIndex(int index)
    {
        ScrollToIndex(index, 0.0f);
    }

    /// <summary>
    /// 跳转到list指定index对应的item
    /// </summary>
    /// <param name="index">item的index</param>
    /// <param name="animTime">动画播放时间，传递0则不播放动画</param>
    public void ScrollToIndex(int index, float animTime)
    {
        index = index - 1;
        if (index < 0)
        {
            index = 0;
        }
        else if (index >= _dataLength)
        {
            index = _dataLength - 1;
        }
        float y = this._yCache[index];
        if (_scrollRect.vertical)
        {
            this.ScrollToPosition(0, -y, animTime);
        }
        else {
            this.ScrollToPosition(y, 0, animTime);
        }
        //this.transform.DOScale(
    }

    private void _StopScrollAnim()
    {
        if (_tween != null) {
            _tween.Kill();
            _tween = null;
        }
    }

    /// <summary>
    /// 跳转到list指定位置
    /// 纵向list会忽略x参数，横向list会忽略y参数
    /// </summary>
    /// <param name="x">横向位置</param>
    /// <param name="y">纵向位置</param>
    /// <param name="animTime">动画播放时间，传递0则不播放动画</param>
    public void ScrollToPosition(float x, float y, float animTime)
    {
        Vector2 position = Vector2.zero;
        if (_scrollRect.vertical) {
            float maxY = _content.rect.height - _viewPort.rect.height;
            position.y = Mathf.Clamp01(1 - y / maxY);
        }
        else{
            float maxX = _content.rect.width - _viewPort.rect.width;
            position.x = Mathf.Clamp01(x / maxX);
        }
       
        if (animTime == 0)
        {
            _scrollRect.normalizedPosition = position;
        }
        else {
            _StopScrollAnim();
            //transform.DOMoveX(
            _tween = DOTween.To(() => _scrollRect.normalizedPosition, xx => _scrollRect.normalizedPosition = xx, position, animTime).SetEase(Ease.OutCubic);
        }
    }

    public RectTransform GetPrefabFromList(int index)
    {
        return itemPrefabList[index];
    }
}


