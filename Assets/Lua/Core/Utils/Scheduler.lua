module("Scheduler", package.seeall)


-- local _schedulerCache = {}
-- local _idx = 0


-- function Schedule( nHandler, fInterval, data )

-- 	_idx = _idx + 1
-- 	local idx = _idx

-- 	_schedulerCache[idx] = {loop = true, }

-- 	local function cb(  )
-- 		coroutine.wait(fInterval)

-- 		while _schedulerCache[idx].loop do
-- 			if type(nHandler) == "function" then
-- 				nHandler(data)
-- 			end
-- 			coroutine.wait(fInterval)
-- 		end

-- 		_schedulerCache[idx] = nil
-- 	end

-- 	_schedulerCache[idx].cb = cb

-- 	coroutine.start(cb)

-- 	return idx
-- end

-- function ScheduleOnce( nHandler, fInterval, data )

-- 	local function cb(  )
-- 		coroutine.wait(fInterval)
-- 		if type(nHandler) == "function" then
-- 			nHandler(data)
-- 		end
-- 	end

-- 	coroutine.start(cb)
-- end

-- function Unschedule( entryId )
-- 	_schedulerCache[entryId].loop = false
-- end

-- function UnscheduleAll(  )
-- 	for k,v in pairs(_schedulerCache) do
-- 		_schedulerCache[k].loop = false
-- 	end
-- end

local SchedulerCS = LuaHelper.Scheduler


function Schedule( nHandler, fInterval, data, gameObject )
	local cb = function (  )
		nHandler(data)
	end

	return SchedulerCS:Schedule(cb, gameObject or SchedulerCS.gameObject, fInterval, -1, 0)
end

function ScheduleOnce( nHandler, fInterval, data, gameObject )
	local cb = function (  )
		nHandler(data)
	end
	
	return SchedulerCS:ScheduleOnce(cb, gameObject or SchedulerCS.gameObject, fInterval)
end

function Unschedule( entryId, gameObject )
	SchedulerCS:Unschedule(entryId, gameObject or SchedulerCS.gameObject)
end

function UnscheduleByTarget( gameObject )
	SchedulerCS:UnscheduleByTarget(gameObject)
end

function UnscheduleAll(  )
	SchedulerCS:UnscheduleAll()
end


