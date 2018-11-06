HttpTest = class("HttpTest", ViewBaseLogic)

function HttpTest:Start(  )
	ulog("HttpTest:Start -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
	self._luaBehaviour:AddClick("BG/Request", self.Request)
end

local IP = "http://192.168.2.173:80/version"
local PORT = "23443"


local function request( data )
	local www = UnityEngine.WWW.New(IP, data)

	coroutine.www(www)

	if www.error then
		ulog("Error: " .. www.error)
	else
		ulog(www.text)
	end
end

function HttpTest:Request(  )
	ulog("*** HttpTest:Request ***")

	local form = UnityEngine.WWWForm.New()
	form:AddField("bundleId", "com.st.tank")
	form:AddField("build", 1)
	form:AddField("deviceId", "deviceId_123456")
	form:AddField("deviceType", "windows")
	form:AddField("subPackage", 0)
	form:AddField("whiteList", "")


    coroutine.start(request, form)

end