using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using LuaInterface;

public class TouchEvent : MonoBehaviour, IEventSystemHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IBeginDragHandler, IInitializePotentialDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IScrollHandler, IUpdateSelectedHandler, ISelectHandler, IDeselectHandler, IMoveHandler, ISubmitHandler, ICancelHandler {
	private LuaTable lua = null;

	private LuaFunction luaOnBeginDrag = null;
	private LuaFunction luaOnCancel = null;
	private LuaFunction luaOnDeselect = null;
	private LuaFunction luaOnDrag = null;
	private LuaFunction luaOnDrop = null;
	private LuaFunction luaOnEndDrag = null;
	private LuaFunction luaOnInitializePotentialDrag = null;
	private LuaFunction luaOnMove = null;
	private LuaFunction luaOnPointerClick = null;
	private LuaFunction luaOnPointerDown = null;
	private LuaFunction luaOnPointerEnter = null;
	private LuaFunction luaOnPointerExit = null;
	private LuaFunction luaOnPointerUp = null;
	private LuaFunction luaOnScroll = null;
	private LuaFunction luaOnSelect = null;
	private LuaFunction luaOnSubmit = null;
	private LuaFunction luaOnUpdateSelected = null;

	void Start(){
		LuaBehaviour lb = GetComponent<LuaBehaviour> ();
		if (lb) {
			lua = lb.Lua;

			luaOnBeginDrag = lua.GetLuaFunction ("OnBeginDrag");
			luaOnCancel = lua.GetLuaFunction ("OnCancel");
			luaOnDeselect = lua.GetLuaFunction ("OnDeselect");
			luaOnDrag = lua.GetLuaFunction ("OnDrag");
			luaOnDrop = lua.GetLuaFunction ("OnDrop");
			luaOnEndDrag = lua.GetLuaFunction ("OnEndDrag");
			luaOnInitializePotentialDrag = lua.GetLuaFunction ("OnInitializePotentialDrag");
			luaOnMove = lua.GetLuaFunction ("OnMove");
			luaOnPointerClick = lua.GetLuaFunction ("OnPointerClick");
			luaOnPointerDown = lua.GetLuaFunction ("OnPointerDown");
			luaOnPointerEnter = lua.GetLuaFunction ("OnPointerEnter");
			luaOnPointerExit = lua.GetLuaFunction ("OnPointerExit");
			luaOnPointerUp = lua.GetLuaFunction ("OnPointerUp");
			luaOnScroll = lua.GetLuaFunction ("OnScroll");
			luaOnSelect = lua.GetLuaFunction ("OnSelect");
			luaOnSubmit = lua.GetLuaFunction ("OnSubmit");
			luaOnUpdateSelected = lua.GetLuaFunction ("OnUpdateSelected");
		}
	}

	private void DestroyLuaFunc(LuaFunction fun){
		if (fun != null) {
			fun.Dispose ();
		}
	}

	void OnDestroy(){
		DestroyLuaFunc (luaOnBeginDrag);
		DestroyLuaFunc (luaOnCancel);
		DestroyLuaFunc (luaOnDeselect);
		DestroyLuaFunc (luaOnDrag);
		DestroyLuaFunc (luaOnDrop);
		DestroyLuaFunc (luaOnEndDrag);
		DestroyLuaFunc (luaOnInitializePotentialDrag);
		DestroyLuaFunc (luaOnMove);
		DestroyLuaFunc (luaOnPointerClick);
		DestroyLuaFunc (luaOnPointerDown);
		DestroyLuaFunc (luaOnPointerEnter);
		DestroyLuaFunc (luaOnPointerExit);
		DestroyLuaFunc (luaOnPointerUp);
		DestroyLuaFunc (luaOnScroll);
		DestroyLuaFunc (luaOnSelect);
		DestroyLuaFunc (luaOnSubmit);
		DestroyLuaFunc (luaOnUpdateSelected);
	}



	private void CallLua(LuaFunction fun, BaseEventData eventData){
		if (fun != null) {
			fun.BeginPCall ();
			fun.Push(lua);
			fun.Push(eventData);
			fun.PCall ();
			fun.EndPCall ();
		}
	}

	public void OnBeginDrag(PointerEventData eventData){
		//Debug.Log ("*********  OnBeginDrag  ************");
		CallLua (luaOnBeginDrag, eventData);
	}

	public void OnCancel (BaseEventData eventData){
		//Debug.Log ("*********  OnCancel  ************");
		CallLua (luaOnCancel, eventData);
	}

	public void OnDeselect (BaseEventData eventData){
		//Debug.Log ("*********  OnDeselect  ************");
		CallLua (luaOnDeselect, eventData);
	}

	public void OnDrag (PointerEventData eventData){
		//Debug.Log ("*********  OnDrag  ************");
		CallLua (luaOnDrag, eventData);
	}

	public void OnDrop (PointerEventData eventData){
		//Debug.Log ("*********  OnDrop  ************");
		CallLua (luaOnDrop, eventData);
	}

	public void OnEndDrag (PointerEventData eventData){
		//Debug.Log ("*********  OnEndDrag  ************");
		CallLua (luaOnEndDrag, eventData);
	}

	public void OnInitializePotentialDrag (PointerEventData eventData){
		//Debug.Log ("*********  OnInitializePotentialDrag  ************");
		CallLua (luaOnInitializePotentialDrag, eventData);
	}

	public void OnMove (AxisEventData eventData){
		//Debug.Log ("*********  OnMove  ************");
		CallLua (luaOnMove, eventData);
	}

	public void OnPointerClick (PointerEventData eventData){
		//Debug.Log ("*********  OnPointerClick  ************");
		CallLua (luaOnPointerClick, eventData);
	}

	public void OnPointerDown (PointerEventData eventData){
		//Debug.Log ("*********  OnPointerDown  ************");
		CallLua (luaOnPointerDown, eventData);
	}

	public void OnPointerEnter (PointerEventData eventData){
		//Debug.Log ("*********  OnPointerEnter  ************");
		CallLua (luaOnPointerEnter, eventData);
	}

	public void OnPointerExit (PointerEventData eventData){
		//Debug.Log ("*********  OnPointerExit  ************");
		CallLua (luaOnPointerExit, eventData);
	}

	public void OnPointerUp (PointerEventData eventData){
		//Debug.Log ("*********  OnPointerUp  ************");
		CallLua (luaOnPointerUp, eventData);
	}

	public void OnScroll (PointerEventData eventData){
		//Debug.Log ("*********  OnScroll  ************");
		CallLua (luaOnScroll, eventData);
	}

	public void OnSelect (BaseEventData eventData){
		//Debug.Log ("*********  OnSelect  ************");
		CallLua (luaOnSelect, eventData);
	}

	public void OnSubmit (BaseEventData eventData){
		//Debug.Log ("*********  OnSubmit  ************");
		CallLua (luaOnSubmit, eventData);
	}

	public void OnUpdateSelected (BaseEventData eventData){
		//Debug.Log ("*********  OnUpdateSelected  ************");
		CallLua (luaOnUpdateSelected, eventData);
	}
}
