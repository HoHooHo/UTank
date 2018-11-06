UITest = class("UITest", ViewBaseLogic)
UITest.fullScreen = true
UITest.layer = SceneHelper.LAYER_TYPE.BASE_LAYER

function UITest:Awake(  )
	ulog("UITest:Awake -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)

	UIManager.Init(UnityEngine.GameObject.Find("Root").transform)
end

function UITest:OnEnable(  )
	ulog("UITest:OnEnable -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)
end

function UITest:Start(  )
	ulog("UITest:Start -->> " .. "id = " .. self.__id .. "  object.name = " .. self._gameObject.name .. "  Instance.name = " .. self.__instanceName)


	self._luaBehaviour:AddClick("BG/LifeCycle", self.OnLifeCycleClick)
	self._luaBehaviour:AddClick("BG/Button", self.OnButtonClick)
	self._luaBehaviour:AddClick("BG/EventSystem", self.OnEventSystemClick)
	self._luaBehaviour:AddClick("BG/ListTestButton", self.OnListTestClick)
	self._luaBehaviour:AddClick("BG/Scheduler", self.OnSchedulerClick)
	self._luaBehaviour:AddClick("BG/Socket", self.OnSocketClick)
	self._luaBehaviour:AddClick("BG/Http", self.OnHttpClick)
	self._luaBehaviour:AddClick("BG/ImageEffectButton", self.OnImageEffectClick)
	self._luaBehaviour:AddClick("BG/TouchEvent", self.OnTouchEventClick)
	self._luaBehaviour:AddClick("BG/3D", self.On3DClick)
	self._luaBehaviour:AddClick("BG/NestedPrefabs", self.OnNestedPrefabsClick)
	self._luaBehaviour:AddClick("BG/UIStack", self.OnUIStackClick)

	ViewStack.PushView(self)
end


function UITest:OnLifeCycleClick( btn )
	ulog("========== UITest:OnLifeCycleClick ==========")

	UIManager.Open("TestLifeCycle")
end


function UITest:OnButtonClick( btn )
	ulog("========== UITest:OnButtonClick ==========")

	
	UIManager.Open("TestButton", nil, nil, "1")
	-- UIManager.Open("TestButton", nil, nil, "1")
end

function UITest:OnEventSystemClick( btn )
	ulog("========== UITest:OnEventSystemClick ==========")
	UIManager.Open("TestEvent", nil, nil)
end

function UITest:OnListTestClick( btn )
	print("========== UITest:OnListTestClick ==========")
	UIManager.Open("ListTest", nil, nil, nil, "list/ListTest")
end

function UITest:OnSchedulerClick( btn )
	ulog("========== UITest:OnSchedulerClick ==========")
	UIManager.Open("TestScheduler", nil, nil)
end

function UITest:OnSocketClick( btn )
	ulog("========== UITest:OnSocketClick ==========")
	UIManager.Open("TestSocket", nil, nil)
end

function UITest:OnHttpClick( btn )
	ulog("========== UITest:OnHttpClick ==========")
	UIManager.Open("TestHttp", nil, nil)
end

function UITest:OnImageEffectClick( btn )
	UIManager.Open("ImageEffectTest", nil, nil, nil, "imageeffect")
end

function UITest:OnTouchEventClick( btn )
	ulog("========== UITest:OnTouchEventClick ==========")
	UIManager.Open("TestTouchEvent", nil, nil)
end

function UITest:On3DClick( btn )
	ulog("========== UITest:On3DClick ==========")
	UIManager.Open("Test3D", nil, nil)
end

function UITest:OnNestedPrefabsClick( btn )
	ulog("========== UITest:OnNestedPrefabsClick ==========")
	UIManager.Open("TestNestedPrefabs", nil, nil)
end

function UITest:OnUIStackClick( btn )
	UIManager.Open("StackFullView1", nil, nil, nil, "scenestack")
	
end

function UITest:OnCloseClick( btn )
	ulog(" close ")
	--UIManager.OnTestClose()
	ViewStack.DoAndroidBack()
end


function UITest:OnHomeClick( btn )
	ViewStack.PopToHome()
end