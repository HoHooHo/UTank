module("UIManager", package.seeall)

_root = nil

local _cache = {}

local _tempGo = nil

function Init( root )
	_root = root
	SceneHelper.Init(root)
	ViewStack.Init(root)
end

function Open( prefab, cb, parentLogic, param, abName )
	local abName = string.lower(abName or prefab)
	local assetName = prefab


	local function onLoad( obj )
		SceneHelper.SetTouchable(true)
		local go = UnityEngine.Object.Instantiate(obj[0])
		--_cache[go] = abName

		local lb = go:GetComponent("LuaBehaviour")
		local viewLogic = nil
		if lb then
			viewLogic = lb.Lua
			viewLogic.abName = abName
			if param then
				viewLogic:Ctor(param)
			end
		end

		if parentLogic then
			local transfrom = go.transform
			transfrom:SetParent(parentLogic._transform, false)
			parentLogic:addChildLogic(viewLogic)
		else
			ViewStack.PushView(viewLogic)
		end
		-- local localScale = transfrom.localScale
		-- transfrom.localScale = Vector3.New(localScale.x * Win.Scale.x, localScale.y * Win.Scale.y, localScale.z * Win.Scale.z)
		-- transfrom.localPosition = Vector3.New(0, 0, 0)

		-- ulog("Win.Scale.x = " .. Win.Scale.x .. "  " .. "Win.Scale.y = " .. Win.Scale.y .. "  " .. "Win.Scale.z = " .. Win.Scale.z)
		-- ulog("Win.Size.width = " .. Win.Size.width .. "  " .. "Win.Size.height = " .. Win.Size.height)

		--_tempGo = go

		if cb then
			cb(go)
		end
	end
	SceneHelper.SetTouchable(false)
	ResManager:LoadPrefab(abName, {assetName}, onLoad)
end

function Close( viewLogic )
	-- local abName = _cache[gameObject]
	-- _cache[gameObject] = nil


	-- local lb = gameObject:GetComponent("LuaBehaviour")
	-- if lb then
	-- 	lb.Lua:PreDestroy()
	-- end

	-- UnityEngine.GameObject.Destroy(gameObject)

	-- ResManager:UnloadPrefab(abName)
	if viewLogic.parentLogic then
		viewLogic:Dispose()
	else
		ViewStack.PopView(viewLogic)
	end
end

function OnTestClose(  )
	if _tempGo then
		Close(_tempGo)
		_tempGo = nil
	end
end