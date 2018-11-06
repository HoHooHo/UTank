ViewBaseLogic = class("ViewBaseLogic")

ViewBaseLogic.netListenerCache = {}
ViewBaseLogic.webListenerCache = {}
ViewBaseLogic.eventListenerCache = {}
ViewBaseLogic.childLogics = {}


-- gameObject transform luaBehaviour 已经 赋值到 self._gameObject self._transform self._luaBehaviour

function ViewBaseLogic:IAwake(  )
	self:Awake()
end

function ViewBaseLogic:IOnEnable(  )
	self:OnEnable()
end

function ViewBaseLogic:AutoInit(  )
	self.netListenerCache = {}
	self.webListenerCache = {}
	self.eventListenerCache = {}
end

function ViewBaseLogic:Ctor( data )
	ulog("ViewBaseLogic:Ctor( data )-->>" .. data)
end

function ViewBaseLogic:IStart(  )
	self:Start()
end


function ViewBaseLogic:Awake(  )
	ulog("ViewBaseLogic:Awake -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
end

function ViewBaseLogic:OnEnable(  )
	ulog("ViewBaseLogic:OnEnable -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
end

function ViewBaseLogic:Start(  )
	ulog("ViewBaseLogic:Start -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
end


-- function ViewBaseLogic:FixedUpdate(  )
	-- ulog("ViewBaseLogic:FixedUpdate -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
-- end

-- function ViewBaseLogic:Update(  )
	-- ulog("ViewBaseLogic:Update -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
-- end

-- function ViewBaseLogic:LateUpdate(  )
	-- ulog("ViewBaseLogic:LateUpdate -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
-- end

-- function ViewBaseLogic:OnGUI(  )
-- 	ulog("ViewBaseLogic:OnGUI -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
-- end

function ViewBaseLogic:IOnDisable(  )
	self:OnDisable()
end

function ViewBaseLogic:PreDestroy(  )
	self:ClearSchedule()
	self:ClearEventListener()
end

function ViewBaseLogic:IOnDestroy(  )
	self:OnDestroy()
end

function ViewBaseLogic:OnDisable(  )
	ulog("ViewBaseLogic:OnDisable -->> " .. "id = " .. self.__id .. "  Instance.name = " .. self.__instanceName)
end

function ViewBaseLogic:OnDestroy(  )
	ulog("ViewBaseLogic:Destroy -->> " .. "id = " .. self.__id .. "  Instance.name = " .. self.__instanceName)
end


function ViewBaseLogic:Refresh( data )
	self:OnRefresh(data)
end

function ViewBaseLogic:OnRefresh( data )
	ulog("ViewBaseLogic:OnRefresh -->> " .. "id = " .. self.__id .. "  Instance.name = " .. self.__instanceName)
end

function ViewBaseLogic:Find( name )
	return self._gameObject.Find(name)
end

function ViewBaseLogic:GetComponent( name, objName )
	if objName then
		return self:Find(objName):GetComponent(name)
	else
		return self:GetComponent(name)
	end
end

function ViewBaseLogic:Close(  )
	UIManager.Close(self)
end

function ViewBaseLogic:SetActive( active )
	self._gameObject:SetActive(active)
end

function ViewBaseLogic:GetActive(  )
	return self._gameObject.activeInHierarchy
end


--由引擎内部调用
--外部使用请使用Close()
function ViewBaseLogic:Dispose()
	--todo 调用所有childLogic的Dispose
	self:clearChildLogic()
	if self.parentLogic then
		self.parentLogic:removeChildLogic(self)
	end

	self:PreDestroy()
	UnityEngine.GameObject.Destroy(self._gameObject)    
	ResManager:UnloadPrefab(self.abName)
end

function ViewBaseLogic:addChildLogic( logic )
	if logic == nil then
		return
	end
	self.childLogics[#self.childLogics + 1] = logic
	logic.parentLogic = self
end

function ViewBaseLogic:removeChildLogic( logic )
	for i,v in pairs(self.childLogics) do
		if v == logic then
			v.parentLogic = nil
			self.childLogics[i] = nil
			break
		end
	end
end

function ViewBaseLogic:clearChildLogic(  )
	for i,v in pairs(self.childLogics) do
		v.parentLogic = nil
		v:Dispose()
	end

	self.childLogics = {}
end


-- 集成EventSystem
local function CheckEventListener( cache, listener )
	for k,v in pairs(cache) do
		if v.lis == listener then
			ulog("ViewBaseLogic:RegisterEventListener: cannot repeat registered event")
			return false
		end
	end

	return true
end


function ViewBaseLogic:RegisterEventListener( eventType, listener )
	if self.eventListenerCache[eventType] == nil then
		self.eventListenerCache[eventType] = {}
	end

	if CheckEventListener(self.eventListenerCache[eventType], listener) then
		local function closure_callback( data )
			listener( self, data )
		end

		table.insert(self.eventListenerCache[eventType], { et = eventType, lis = listener, cc = closure_callback })

		EventSystem.RegisterListener(eventType, closure_callback)
	end

end

function ViewBaseLogic:PostEvent(eventType, data)
	EventSystem.Post(eventType, data)
end

function ViewBaseLogic:UnregisterEventListener(eventType, listener)
	local caches = self.eventListenerCache[eventType]

	if caches == nil then
		ulog("ViewBaseLogic:UnregisterEventListener: nothing has registered with "..eventType)
		return
	end

	local unregister = false

	for k,v in pairs(caches) do
		if v.lis == listener then
			EventSystem.UnregisterListener(eventType, v.cc)
			table.remove(caches, k)
			unregister = true
			ulog("ViewBaseLogic:UnregisterEventListener: unregisterEventListener "..eventType.." success")
			break
		end
	end

	if not unregister then
		ulog("ViewBaseLogic:UnregisterEventListener: has not registered with "..eventType)
	end
end

function ViewBaseLogic:ClearEventListener(  )
	for eventType, listeners in pairs(self.eventListenerCache) do
		for k,v in pairs(listeners) do
			EventSystem.UnregisterListener(eventType, v.cc)
		end
	end
end


-- 集成Scheduler
function ViewBaseLogic:Schedule( nHandler, fInterval, data )
	return Scheduler.Schedule(nHandler, fInterval, data, self._gameObject)
end

function ViewBaseLogic:ScheduleOnce( nHandler, fInterval, data )
	return Scheduler.ScheduleOnce(nHandler, fInterval, data, self._gameObject)
end

function ViewBaseLogic:Unschedule( entryId )
	Scheduler.Unschedule(entryId, self._gameObject)
end

function ViewBaseLogic:ClearSchedule(  )
	Scheduler.UnscheduleByTarget(self._gameObject)
end


-- 嵌套使用prefab时  根据挂载子prefab的名字 获取 子prefab
function ViewBaseLogic:GetChildPrefab( nodeName )
	-- 获取 挂在了 PrefabInPrefab 组件的节点
	local node = self._gameObject.Find(nodeName)

	-- 获取 PrefabInPrefab组件
	local component = node:GetComponent("PrefabInPrefab")

	-- 通过 PrefabInPrefab组件的变量 获取子预设体
	local childPrefab = component.Child

	
	-- 获取子预设体的LuaBehaviour组件
	local lb = childPrefab:GetComponent("LuaBehaviour")

	-- 获取子预设体的lua对象
	local lua = lb.Lua

	return childPrefab, lua
end

-- 动画事件回调
function ViewBaseLogic:OnAnimationEvent(eventName)
	ulog("========== ViewBaseLogic:OnAnimationEvent ==========")
	ulog(eventName)
end