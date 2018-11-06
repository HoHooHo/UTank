-- module("SocketProxy", package.seeall)

-- -- 连接状态 
-- --    0: 未初始化
-- --    1: 已经连接
-- --    2: 正在连接 
-- --    3: 取消连接
-- --   -1: 连接失败 
-- SOCKET_STATE = 
-- {
--     NONE = 0,
--     CONNECTED = 1,
--     CONNECTING = 2,
--     CANCEL = 3,
--     CONNECT_FAILE = -1,
-- }

-- local _socket = nil 

-- local _receiveState = false

-- local _socketState = SOCKET_STATE.NONE

-- local _connectTime = 0
-- local CONNECT_TIME_LIMIT = 20

-- local _retryCount = 0
-- local RETRY_MAX_COUNT = 4

-- local _sendBuffer = {}
-- local _receiveBuffer = ""

-- local function receive(partial)
--     if (partial == nil) then partial = {} end

--     -- local err = "timeout"
--     local err = nil
--     local value = ""
--     while value ~= nil do
--         _receiveBuffer = _receiveBuffer .. value
--         value, err, partial = _socket:receive("*o")
--     end

--     local headlen = 8
--     while string.len(_receiveBuffer) >= headlen do
--         _retryCount = 0
--         local length = pb.readInt(_receiveBuffer)
--         local pkgType = pb.readShort(_receiveBuffer, 4)
--         local index = pb.readShort(_receiveBuffer, 6)
--         local requireLen = length - 4
--         if Config and Config.Debug then
--             logW("receive header: length: "..tostring(length)..", pkgType: "..tostring(getmetatable(_pkgMap:GetPkgByType(pkgType))._descriptor.name)..", index: "..tostring(index) .. ",  string.len(_receiveBuffer): " .. string.len(_receiveBuffer) .. ",  " .. tostring(string.len(_receiveBuffer) - headlen >= requireLen))
--         end
--         if string.len(_receiveBuffer) - headlen >= requireLen then
--             _receiveState = false
--             --去掉包头
--             _receiveBuffer = string.sub(_receiveBuffer, headlen + 1)
--             --截取数据
--             local response = string.sub(_receiveBuffer, 1, requireLen)
--             _receiveBuffer = string.sub(_receiveBuffer, requireLen + 1)
--             --取的包头
--             local msg = _pkgMap:GetPkgByType(pkgType)
--             if msg then
--                 msg:ParseFromString(response)
--                 if _netHandler then
--                     _netHandler( pkgType , msg )
--                 end
--             else
--                 error("\n***ERROR*** SocketProxy ***ERROR***\n" .. tostring(pkgType) )
--             end
--         else
--             break
--         end
--     end

--     return err
-- end

-- local function step(  )
--     if (_socket ~= nil) then
--         -- 状态为CONNECTING, 表示正在进行连接,此时需检查socket读写状态
--         if (_socketState == SOCKET_STATE.CONNECTING) then
--             local ar, aw, code = socket.select({_socket}, {_socket}, 0)
--             if (#ar>0 or #aw > 0) then
--                 logW("Socket Connected...")
--                 _socketState = SOCKET_STATE.CONNECTED
--                 -- sendAuth()
--                 LoginAuth.sendAuth( _ip, _port, _socket, getUID(), getSecret(), reconnectNet, CONNECT_TIME_LIMIT )
--             else
--                 if (os.time() - _connectTime > CONNECT_TIME_LIMIT) then
--                     -- 上次连接超过5s, 重新创建socket
--                     logW("time more than " .. CONNECT_TIME_LIMIT .. "s, reconnect again...")
--                     endNet( true )
--                 end
--             end
--         -- 状态为CONNECTED, 连接能正常读写的场合, 测试接受数据
--         elseif (_socketState == SOCKET_STATE.CONNECTED) then

--             local receiveRet = receive()

--             if receiveRet == "timeout" then
--                 --is ok
--                 if #_sendBuffer > 0 then
--                     local retError = 0

--                     for i,v in ipairs(_sendBuffer) do

--                         if Config.Debug then 
--                             local str = "@@@  REDY SEND MSG msgName = " .. tostring(getmetatable(v.pkg)._descriptor.name) .. " msgType = " .. _pkgMap:GetPkgType(v.pkg) .. " @@@"
--                             logC( logbit(F_G, F_I), str)
--                             XDebug.xMsg(0, str)
--                         end

--                         local res = _socket:send( v.finalString )

--                         if Config.Debug then 
--                             local str = "@@@  REAL SEND MSG msgName = " .. tostring(getmetatable(v.pkg)._descriptor.name) .. " msgType = " .. _pkgMap:GetPkgType(v.pkg) .. "  res = " .. tostring(res) .." @@@"
--                             logC( logbit(F_G, F_I), str)
--                             XDebug.xMsg(0, str)
--                         end

--                         if res == nil or res <= 0 then
--                             retError = 1
--                             break
--                         end
--                     end

--                     if retError > 0 then
--                        EventSystem.dispatchEvent(EventType.Socket_Send_Error)
--                     end

--                     _sendBuffer = {}

--                     if retError > 0 then
--                         NetLoadingViewLogic.clear()
--                         endNet( true )
--                     end
--                 end
--             elseif receiveRet == "closed" then
--                 cclog("************* socket closed *************")
--                 if _receiveState then
--                     endNet()
--                     if _errorHandler then
--                         _errorHandler( receiveRet )
--                     end
--                 end
--             else
--                 if _receiveState then
--                     cclog("#####  UNKNOWN SOCKET ERROR  #####" .. tostring(receiveRet))
--                     endNet( true )
--                 end
--             end
--         end
--     end
-- end

-- local function init(  )
--     _socketState = SOCKET_STATE.NONE
--     _retryCount = 0

--     Scheduler.Schedule( step, 0.03 )

--     -- LoginAuth.init()
-- end

-- local function ipv6_only( ip )
--     if _G._IS_IOS_REVIEW then
--         local addrifo, err = socket.dns.getaddrinfo(ip)
--         if addrifo ~= nil then
--             for k,v in pairs(addrifo) do
--                 if v.family == "inet6" then
--                     return true
--                 end
--             end
--         end
--     end

--     return false
-- end

-- local function connect()
--     if (_socketState == SOCKET_STATE.NONE) then

--         if ipv6_only(_ip) then
--             -- Global.showTips("ipv6")
--             _socket = socket.tcp6()
--         else
--             -- Global.showTips("ipv4")
--             _socket = socket.tcp()
--         end

--         _socket:settimeout(0)
--         local ret, error = _socket:connect(_ip, _port)

--         _socketState = SOCKET_STATE.CONNECTING
--         _connectTime = os.time()
--         cclog("connect to ip: ".._ip..", port: ".._port)

--         NetLoadingViewLogic.show()
--     end
-- end

-- function startNet( ip, port, pkgMap )
--     initProxy( ip, port, pkgMap )
--     init()
--     connect()
-- end

-- function endNet( reconnect )
--     local oldState = _socketState
    
--     if (_socketState == SOCKET_STATE.CONNECTED or _socketState == SOCKET_STATE.CONNECTING) then

--         _socketState = SOCKET_STATE.NONE
--         _socket:close()
--         _socket = nil
--         -- _retryCount = 0
--         cclog("Socket closed success")
--     else
--         cclog("Socket is already closed")
--     end

--     if reconnect then
--         tryReconnect()
--     end
-- end

-- local function generateFinalPkg(pkg)
--     local pkgType = _pkgMap:GetPkgType(pkg)

--     _pdString:clear()
--     _pbFinalString:clear()

--     pkg:SerializeToIOString(_pdString)

--     _pbFinalString:writeShort(_pdString:__len() + 6)
--     _pbFinalString:writeShort(pkgType)
--     _pbFinalString:writeInt(1)
--     _pbFinalString:write(_pdString:__tostring())
    
--     return _pbFinalString:__tostring()
-- end

-- local function _send( pkg )
--     checkSocket(  )

--     local finalPkg = {}
--     finalPkg.pkg = pkg
--     finalPkg.finalString = generateFinalPkg(pkg)
--     _sendBuffer[#_sendBuffer+1] = finalPkg

--     _receiveState = true
-- end

-- function send( pkg )
--     -- checkNet( _send, pkg )
--     _send( pkg )
-- end

-- function clear (  )
--     cclog("==== ****    CLEAR SOCKET    **** ====")
--     endNet()
-- end