AssetBundleLuaTest = class("AssetBundleLuaTest", ViewBaseLogic)

function AssetBundleLuaTest:Awake(  )
	ulog("AssetBundleLuaTest:Awake -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name" .. self.__instanceName)
end

function AssetBundleLuaTest:OnEnable(  )
	ulog("AssetBundleLuaTest:OnEnable -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name" .. self.__instanceName)
end

function AssetBundleLuaTest:Start(  )
	ulog("AssetBundleLuaTest:Start -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name" .. self.__instanceName)
	Scheduler.scheduleOnce(function (  )
		
		UIManager.close(self._gameObject)
	end, 2)
end