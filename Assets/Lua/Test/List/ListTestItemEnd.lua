ListTestItemEnd = class("ListTestItemEnd", ViewBaseLogic)

local _list

function ListTestItemEnd:Awake(  )
	self.msgText = self._transform:Find("Text"):GetComponent('Text');
end

function ListTestItemEnd:OnRefreshData(tableData,index  )
	local data = tableData[index]
	self.msgText.text = "test " .. data
end