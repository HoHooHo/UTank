SubPrefabTest = class("SubPrefabTest", ViewBaseLogic)

function SubPrefabTest:Start(  )
	ulog("SubPrefabTest:Start -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
	self.text = self._transform:Find("BG/Text"):GetComponent("Text")

	-- self.text.text = self.__instanceName
end

function SubPrefabTest:SetName( str )
	self.text.text = str
end
