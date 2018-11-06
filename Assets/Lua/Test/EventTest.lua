EventTest = class("EventTest", ViewBaseLogic)

function EventTest:Awake(  )
	ulog("EventTest:Awake -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
end

function EventTest:OnEnable(  )
	ulog("EventTest:OnEnable -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
end

function EventTest:Start(  )
	ulog("EventTest:Start -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)

	self._luaBehaviour:AddClick("BG/Register", self.OnRegisterClick)
	self._luaBehaviour:AddClick("BG/Unregister1", self.OnUnregister1Click)
	self._luaBehaviour:AddClick("BG/Unregister2", self.OnUnregister2Click)
	self._luaBehaviour:AddClick(self._transform:Find("BG/Post").gameObject, self.OnPostClick)
end

local function OnEvent1( self, data )
	ulog("OnEvent1  " .. data)
end

local function OnEvent2( self, data )
	ulog("OnEvent2  " .. data)
end

function EventTest:OnRegisterClick( btn )
	ulog("OnRegisterClick:  " .. btn.name)

	-- EventSystem.RegisterListener(EventType.TEST, OnEvent1)
	-- EventSystem.RegisterListener(EventType.TEST, OnEvent2)

	self:RegisterEventListener(EventType.TEST, OnEvent1)
	self:RegisterEventListener(EventType.TEST, OnEvent2)
end

function EventTest:OnPostClick( btn )
	ulog("OnPostClick:  " .. btn.name)
	-- EventSystem.Post(EventType.TEST, "ffff")
	self:PostEvent(EventType.TEST, "ffff")
end

function EventTest:OnUnregister1Click( btn )
	ulog("OnRegisterClick:  " .. btn.name)
	-- EventSystem.UnregisterListener(EventType.TEST, OnEvent1)
	self:UnregisterEventListener(EventType.TEST, OnEvent1)
end

function EventTest:OnUnregister2Click( btn )
	ulog("OnRegisterClick:  " .. btn.name)
	-- EventSystem.UnregisterListener(EventType.TEST, OnEvent2)
	self:UnregisterEventListener(EventType.TEST, OnEvent2)
end

function EventTest:OnDestroy( )
	Scheduler.ScheduleOnce(function (  )
		EventSystem.Post(EventType.TEST, "ffff")
	end, 3 )
end