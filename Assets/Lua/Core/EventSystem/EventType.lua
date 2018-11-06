module("EventType", package.seeall)

local EVENT_START = 0
local function getNext(  )
	EVENT_START = EVENT_START + 1
	return EVENT_START
end

TEST = getNext()