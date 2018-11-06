module("EventSystem", package.seeall)

local _eventListenerCache = {}

local KEY_EVENT_TYPE = 1
local KEY_EVENT_CALLBACK = 2

local function CacheListener( eventType, callback )
	if _eventListenerCache[eventType] == nil then
		_eventListenerCache[eventType] = {}
	end

	local cacheObj = {
						[KEY_EVENT_TYPE] = eventType,
						[KEY_EVENT_CALLBACK] = callback,
						-- [KEY_EVENT_LISTENER] = listener
					}

	local caches = _eventListenerCache[eventType]


	for i,v in ipairs(caches) do
		if v[KEY_EVENT_CALLBACK] == callback then
			return false
		end
	end

	table.insert(caches, cacheObj)

	return true
end

function RegisterListener( eventType, callback )

	if callback == nil then
        error("\n\n***ERROR*** callback is nil  ***ERROR***")
	end
	if CacheListener(eventType, callback) then
		ulog("EventSystem: registered event "..eventType.." success")
	else
		ulog("EventSystem: cannot repeat registered event")
	end
end

function UnregisterListener( eventType, callback )

	local caches = _eventListenerCache[eventType]

	if caches == nil then
		ulog("EventSystem: nothing has registered with "..eventType)
		return
	end

	local unregister = false

	for i,v in ipairs(caches) do
		if v[KEY_EVENT_CALLBACK] == callback then
			table.remove(caches, i)
			unregister = true
			ulog("EventSystem: unregisterEventListener "..eventType.." success")
			break
		end
	end

	if not unregister then
		ulog("EventSystem: has not registered with "..eventType)
	end
end

function Post( eventType, data )
	local caches = _eventListenerCache[eventType]

	if caches then
		for i,v in ipairs(caches) do
			if v and v[KEY_EVENT_CALLBACK] then
				v[KEY_EVENT_CALLBACK]( data )
			end
		end
	end

end