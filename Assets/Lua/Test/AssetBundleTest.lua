AssetBundleTest = class("AssetBundleTest", ViewBaseLogic)

function AssetBundleTest:Awake(  )
	ulog("AssetBundleTest:Awake -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name" .. self.__instanceName)
end

function AssetBundleTest:OnEnable(  )
	ulog("AssetBundleTest:OnEnable -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name" .. self.__instanceName)
end


-- function TestArray(array)
--     local len = array.Length
    
--     for i = 0, len - 1 do
--         ulog('Array: '..tostring(array[i]))
--     end

--     local t = array:ToTable()                
    
--     for i = 1, #t do
--         ulog('table: '.. tostring(t[i]))
--     end

--     local iter = array:GetEnumerator()

--     while iter:MoveNext() do
--         ulog('iter: '..iter.Current)
--     end

--     local pos = array:BinarySearch(3)
--     ulog('array BinarySearch: pos: '..pos..' value: '..array[pos])

--     pos = array:IndexOf(4)
--     ulog('array indexof bbb pos is: '..pos)
    
--     return 1, '123', true
-- end            

-- local m = 0

-- local function test( self )

-- 	-- local abName = "file://" .. UnityEngine.Application.streamingAssetsPath .. "/" .. "assetbundle"
-- 	local abName = "assetbundle"
-- 	local assetName = "AssetBundle"

-- 	local function onLoad( obj )

-- 		m = m + 1

-- 		ulog("************************oooooooooooooo*********************")
-- 		ulog(obj.Length)

-- 		local go = UnityEngine.Object.Instantiate(obj[0])
-- 		go.transform:SetParent(self._tranform)
-- 		go.transform.position = Vector3.New(m - 2, m - 2, 0)
-- 	end

-- 	ResManager:LoadPrefab(abName, {assetName}, onLoad)
-- 	ResManager:LoadPrefab(abName, {assetName}, onLoad)
-- end

-- local function test2( self )

-- 	-- local abName = "file://" .. UnityEngine.Application.streamingAssetsPath .. "/" .. "assetbundle"
-- 	local abName = "assetbundle"
-- 	local assetName = "AssetBundle"

-- 	local function onLoad( obj )

-- 		m = m + 1
-- 		ulog("************************ooooooooooooo222222222o*********************")
-- 		obj = obj:ToTable()
-- 		ulog(#obj)
-- 		local go = UnityEngine.Object.Instantiate(obj[1])
-- 		go.transform:SetParent(self._tranform)
-- 		go.transform.position = Vector3.New(m + 2, m + 2, 10)
-- 	end

-- 	ResManager:LoadPrefab(abName, {assetName}, onLoad)
-- 	ResManager:LoadPrefab(abName, {assetName}, onLoad)
-- end

local cb = function ( obj )
	Scheduler.scheduleOnce(function (  )
		-- UIManager.close(obj)
	end, 2)
end

function AssetBundleTest:Start(  )
	ulog("AssetBundleTest:Start -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name" .. self.__instanceName)
	Scheduler.scheduleOnce(function (  )
		UIManager.open("AssetBundle", cb)
		-- UIManager.open("AssetBundle", cb)
		-- UIManager.open("AssetBundle", cb)
	end, 2, self)
	-- Scheduler.scheduleOnce(test2, 4, self)
end