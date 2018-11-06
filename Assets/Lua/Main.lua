LuaHelper = LuaHelperCS.Instance
ResManager = LuaHelper.ResManager

socket = require("socket")



require("Core/Utils/LogHelper")
require("Core/Utils/LangHelper")

require("Core/Utils/extern")
require("Core/Utils/Scheduler")

require("Core/EventSystem/EventType")
require("Core/EventSystem/EventSystem")

require("Core/NetSystem/SocketProxy")

require("Core/ViewCore/ViewBaseLogic")
require("Core/ViewCore/UIManager")
require("Core/ViewCore/SceneHelper")
require("Core/ViewCore/ViewStack")
require("Core/ViewCore/TweenHelper")


require("Test/UITest")

require("Test/LifeCycleTest")
require("Test/ButtonTest")
require("Test/EventTest")

require("Test/SchedulerTest")

require("Test/SocketTest")
require("Test/HttpTest")
require("Test/TouchEventTest")
require("Test/T3DTest")
require("Test/SubPrefabTest")
require("Test/NestedPrefabsTest")

require("Test/AssetBundleTest")
require("Test/AssetBundleLuaTest")

require("Test/List/ListTest")
require("Test/List/ListTestItem")
require("Test/List/ListTest2")
require("Test/List/ListTestItemEnd")
require("Test/ImageEffect/ImageEffect")
require("Test/SceneStack/StackFullView1")
require("Test/SceneStack/StackFullView2")
require("Test/SceneStack/PopView1")
require("Test/SceneStack/PopView2")


ulog("LuaLoader------Main.lua")

