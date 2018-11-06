ButtonTest = class("ButtonTest", ViewBaseLogic)

function ButtonTest:Awake(  )
	ulog("ButtonTest:Awake -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
end

function ButtonTest:OnEnable(  )
	ulog("ButtonTest:OnEnable -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
end

function ButtonTest:Start(  )
	ulog("ButtonTest:Start -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)

	self._luaBehaviour:AddClick("BG/ScriptButton2", self.OnScriptButtonClick)
	self._luaBehaviour:AddClick(self._transform:Find("BG/ScriptButton").gameObject, self.OnScriptButtonClick)
end

function ButtonTest:OnEditorButtonClick( btn )
	ulog("OnEditorButtonClick:  " .. btn.name)

	self._luaBehaviour:RemoveClick(self._transform:Find("BG/ScriptButton2").gameObject)

	self._luaBehaviour:RemoveClick("BG/ScriptButton")
end

function ButtonTest:OnScriptButtonClick( btn )
	ulog("OnScriptButtonClick:  " .. btn.name)
end