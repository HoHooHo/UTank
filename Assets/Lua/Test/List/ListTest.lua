ListTest = class("ListTest", ViewBaseLogic)

local _list
local _listData

function ListTest:Start(  )
	_list = self._transform:Find("list"):GetComponent('UIList');
	 _list.gap = 5
	 _listData = {}
	 for i=1,20 do
	 	table.insert(_listData, i)
	 end
	_list.ListData = _listData

	self._luaBehaviour:AddClick("AddButton", self.OnAddButtonClick)
	self._luaBehaviour:AddClick("DeleteButton", self.OnDeleteButtonClick)
	self._luaBehaviour:AddClick("JumpButton", self.OnJumpButtonClick)
end

function ListTest:OnDestroy(  )
	_list = nil
	_listData = nil
end

function ListTest:OnAddButtonClick( btn )
	table.insert(_listData, 9, 99)
	_list:Reload()
	-- _list.ListData = _listData
end

function ListTest:OnDeleteButtonClick( btn )
	if #_listData > 2 then
		table.remove(_listData, 3)
		_list:Reload()
	end
	

end

function ListTest:OnJumpButtonClick( btn )
	_list:ScrollToIndex(8, 0.3)
end

