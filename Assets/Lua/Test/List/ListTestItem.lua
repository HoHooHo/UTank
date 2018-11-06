ListTestItem = class("ListTestItem", ViewBaseLogic)

local _list

function ListTestItem:Awake(  )
	self.msgText = self._transform:Find("Text"):GetComponent('Text');
end

function ListTestItem:OnRefreshData(tableData,index  )
	local data = tableData[index]
	self.msgText.text = "test " .. data
end