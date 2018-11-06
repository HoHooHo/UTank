ListTest2 = class("ListTest2", ViewBaseLogic)

local _list
local _listData

local _itemPrefab0
local _itemPrefab1
local _itemEndPrefab


local function CellSizeForTable(listData, index)
	if index > #listData then
		return Vector2.New(0, 59)
	end
	if listData[index]  % 2 == 1 then
		return Vector2.New(0, 50)
	else
		return Vector2.New(0, 100)
	end
end

local function NumberOfCellsInTableView()	
	return #_listData + 1
end

local function PrefabAtIndex(listdata, index)
	local data = listdata[index]
	if index > #listdata then
		return _itemEndPrefab
	end
	if data % 2 == 1 then
		return _itemPrefab0
	else
		return _itemPrefab1
	end
end

function ListTest2:Start(  )
	_list = self._transform:Find("list"):GetComponent('UIList');
	 _list.gap = 5
	 _listData = {}
	 for i=1,20 do
	 	table.insert(_listData, i)
	 end

	 _itemPrefab0 = _list:GetPrefabFromList(0)
	 _itemPrefab1 = _list:GetPrefabFromList(1)
	  _itemEndPrefab = _list:GetPrefabFromList(2)

	_list.ListData = _listData

	--如果下面3个函数都指定了，其实可以脱离listData控制
	_list.CellSizeForTable = CellSizeForTable
	_list.NumberOfCellsInTableView = NumberOfCellsInTableView		
	_list.PrefabAtIndex = PrefabAtIndex

	self._luaBehaviour:AddClick("AddButton", self.OnAddButtonClick)
	self._luaBehaviour:AddClick("DeleteButton", self.OnDeleteButtonClick)
	self._luaBehaviour:AddClick("JumpButton", self.OnJumpButtonClick)
end

function ListTest2:OnDestroy(  )
	_list = nil
	_listData = nil
	_itemPrefab0 = nil
	_itemPrefab1 = nil
	_itemEndPrefab = nil
end

function ListTest2:OnAddButtonClick( btn )
	table.insert(_listData, 9, 99)
	_list:Reload()
	-- _list.ListData = _listData
end

function ListTest2:OnDeleteButtonClick( btn )
	if #_listData > 2 then
		table.remove(_listData, 3)
		_list:Reload()
	end
	

end

function ListTest2:OnJumpButtonClick( btn )
	_list:ScrollToIndex(8, 0.3)
end

