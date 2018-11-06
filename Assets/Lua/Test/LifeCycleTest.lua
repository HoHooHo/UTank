LifeCycleTest = class("LifeCycleTest", ViewBaseLogic)

function LifeCycleTest:Awake(  )
	ulog("LifeCycleTest:Awake -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
end

function LifeCycleTest:OnEnable(  )
	ulog("LifeCycleTest:OnEnable -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
end

function LifeCycleTest:Start(  )
	ulog("LifeCycleTest:Start -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
	self._luaBehaviour:AddClick("Image/Button", self.OnClose)
end


--------- WARNNING START----- 这几个函数频繁调用 对性能影响较大   尽量不要使用这几个函数-------
function LifeCycleTest:FixedUpdate(  )
	ulog("LifeCycleTest:FixedUpdate -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
end

function LifeCycleTest:Update(  )
	ulog("LifeCycleTest:Update -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
end

function LifeCycleTest:LateUpdate(  )
	ulog("LifeCycleTest:LateUpdate -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
end

function LifeCycleTest:OnGUI(  )
	ulog("LifeCycleTest:OnGUI -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
end
--------- WARNNING END------------

function LifeCycleTest:OnDisable(  )
	ulog("LifeCycleTest:OnDisable -->> " .. "id = " .. self.__id .. "  Instance.name = " .. self.__instanceName)
end

function LifeCycleTest:OnDestroy(  )
	ulog("LifeCycleTest:Destroy -->> " .. "id = " .. self.__id .. "  Instance.name = " .. self.__instanceName)
end

function LifeCycleTest:OnClose(  )
	self:Close()
end