SocketTest = class("SocketTest", ViewBaseLogic)

function SocketTest:Start(  )
	ulog("SocketTest:Start -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
	self._luaBehaviour:AddClick("BG/Connect", self.ConnectSocket)
	self._luaBehaviour:AddClick("BG/Request", self.Request)
	self._luaBehaviour:AddClick("BG/Close", self.CloseSocket)
end

local IP = "192.168.2.173"
local PORT = "23443"

local _socket = nil
local _receiveBuffer = ""

function SocketTest:ConnectSocket(  )
	ulog("*** SocketTest:ConnectSocket ***")
    _socket = socket.tcp()
    _socket:settimeout(0)
    local ret, error = _socket:connect(IP, PORT)

    ulog("ret = " .. tostring(ret))
    ulog("error = " .. tostring(error))

    local ar, aw, code = socket.select({_socket}, {_socket}, 0)
    ulog("ar = " .. tostring(ar))
    ulog("aw = " .. tostring(aw))
    ulog("code = " .. tostring(code))
    if (#ar>0 or #aw > 0) then
        ulog("Socket Connected...")

    else
        ulog("t*****************************************")
    end


end

function SocketTest:Request(  )
	ulog("*** SocketTest:Request ***")

    local finalbuffer = pb.new_iostring()
    local authString = "a," .. "72339073309625031" .. "," .. "8613910246800"
    finalbuffer:writeShort(#authString)
    -- finalbuffer:write(#authString)
    finalbuffer:write(authString)

    res = _socket:send(finalbuffer:__tostring())
    ulog("res = " .. tostring(res))

    Scheduler.ScheduleOnce(function (  )
	    local err = nil
	    local value = ""
	    while value ~= nil do
	        _receiveBuffer = _receiveBuffer .. value
	        value, err, partial = _socket:receive("*o")
	    end


	    local headlen = 8
	    while string.len(_receiveBuffer) >= headlen do
	        local length = pb.readInt(_receiveBuffer)
	        local pkgType = pb.readShort(_receiveBuffer, 4)
	        local index = pb.readShort(_receiveBuffer, 6)
	        local requireLen = length - 4

	        if string.len(_receiveBuffer) - headlen >= requireLen then
	            --去掉包头
	            _receiveBuffer = string.sub(_receiveBuffer, headlen + 1)
	            --截取数据
	            local response = string.sub(_receiveBuffer, 1, requireLen)
	            _receiveBuffer = string.sub(_receiveBuffer, requireLen + 1)

			    ulog("Recieve ****   " .. pkgType)
	        else
	            break
	        end
	    end

	    ulog("Recieve ****   End")
    end, 5)

end

function SocketTest:CloseSocket(  )
	ulog("*** SocketTest:CloseSocket ***")
	_socket:close()
end