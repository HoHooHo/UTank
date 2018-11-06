module("ViewStack", package.seeall)

root = nil
_topView = nil	--当前场景
isAnimate = nil	--是否在播放切换动画

_viewStack = nil	--场景压栈
_firstScene = nil

local function InTransitionSelector(prevView, newView)
	if newView.PushAnim then
		return newView.PushAnim
	end
	if newView.layer == SceneHelper.LAYER_TYPE.BASE_LAYER or (newView.layer == nil and newView.fullScreen) then
	 	return TweenHelper.TransitionFadeIn
	elseif newView.layer == SceneHelper.LAYER_TYPE.DIALOG_LAYER then
		return TweenHelper.TransitionPopShow
	else
		return TweenHelper.TransitionNone
	end
end

local function OutTransitionSelector(prevView, newView)
	if prevView.PopAnim then
		return prevView.PopAnim
	end
	if prevView.layer == SceneHelper.LAYER_TYPE.BASE_LAYER or (prevView.layer == nil and prevView.fullScreen) then
	 	return TweenHelper.TransitionFadeOut
	elseif prevView.layer == SceneHelper.LAYER_TYPE.DIALOG_LAYER then
		return TweenHelper.TransitionPopClose
	else
		return TweenHelper.TransitionNone
	end
end

local function _DisposeView(view)
	if view.OnBeNotTop then
		view:OnBeNotTop()
	end
	view:Dispose()
end

function Init( root )
	_root = root
	_viewStack = {}

	-- _firstScene = {}
	-- _firstScene._transform = SceneHelper.CreateFullScreenPanel("firstScene")
	-- _firstScene._gameObject = _firstScene._transform.gameObject
	-- PushView(_firstScene)
end

function PushView(view)
	if view == nil then
		return
	end
	table.insert(_viewStack, view)

	local prevView = _topView
	local newView = view
	_topView = newView

	if not prevView then
		--push第一个的时候不需要后续
		local layer = newView.layer or SceneHelper.LAYER_TYPE.BASE_LAYER
		SceneHelper.AddChild(newView._transform, layer)
		if newView.OnBeTop then
			newView:OnBeTop()
		end
		return
	end

	isAnimate = true;
	SceneHelper.SetTouchable(false)

	if prevView.OnBeNotTop then
		prevView:OnBeNotTop()
	end

	--找到实际需要播放出的动画的层
	if newView.fullScreen then
	 	for i=#_viewStack - 1, 1,-1 do
	 		local view = _viewStack[i]
	 		if view.fullScreen then
	 			prevView = view
	 			break
	 		end
	 	end   
     end      

	local outAnim = TweenHelper.TransitionNone
	if newView.fullScreen and prevView.BeCoverAnim then
		outAnim = prevView.BeCoverAnim
	end
	local inAnim = InTransitionSelector(prevView, newView)
	--先播放出的动画
	outAnim(prevView, newView, function()
		--如果新界面为全屏，播放完出的动画之后将所有下面的层设为隐藏
		if newView.fullScreen then
		 	for i=#_viewStack - 1, 1,-1 do
		 		local view = _viewStack[i]
		 		view:SetActive(false)
		 		if view.fullScreen then
		 			break
		 		end
		 	end   
     	end   

     	--加入新界面
     	local layer = newView.layer or SceneHelper.LAYER_TYPE.BASE_LAYER
		SceneHelper.AddChild(newView._transform, layer)
		if newView.OnBeTop then
			newView:OnBeTop()
		end
     	--然后播放进入的动画
     	inAnim(prevView, newView, function()
     		isAnimate = false
			SceneHelper.SetTouchable(true)
			if newView.OnDidShow then
				newView:OnDidShow(true)
			end
     	end)
	end)	
end

--弹出最上层界面
function PopTopView()
	if #_viewStack <= 1 then
		return
	end
	local prevView = _viewStack[#_viewStack]
	local newView = _viewStack[#_viewStack - 1]
	_viewStack[#_viewStack] = nil

	_topView = newView

	isAnimate = true;
	SceneHelper.SetTouchable(false)

	local outAnim = OutTransitionSelector(prevView, newView)
	local inAnim = TweenHelper.TransitionNone


	--先播放出的动画
	outAnim(prevView, newView, function()
		if newView.OnBeTop then
			newView:OnBeTop()
		end

		--如果移除的界面为全屏，将下层的所有界面都设为显示
		if prevView.fullScreen then
			for i=#_viewStack, 1,-1 do
		 		local view = _viewStack[i]
		 		view:SetActive(true)
		 		if view.fullScreen then
		 			newView = view  --将newView改成下层的全屏界面
		 			break
		 		end
		 	end   
		 	if newView.BeTopAnim then
		 		inAnim = newView.BeTopAnim
		 	end
		end
		_DisposeView(prevView)
		
     	--然后播放进入的动画
     	inAnim(prevView, newView, function()
     		isAnimate = false
			SceneHelper.SetTouchable(true)
			if newView.OnDidShow then
				newView:OnDidShow(false)
			end
     	end)
	end)	
end

function ReplaceView()

end

--弹出某一界面
--如果要弹出的不是最上层界面,debug版会报错，release版会将该view以上所有层都移除
function PopView(view)
	if view == _viewStack[#_viewStack] then
		--如果移除的时最上层界面
		PopTopView()
		return
	end
	for i=#_viewStack-1, 2,-1 do
		local theView = _viewStack[i]
		if theView == view then
			PopToTarget(_viewStack[i-1])
			return
		end
	end

	--没找到要移除的view，直接关闭该界面
	_DisposeView(view)
end

--删除某一界面
--和PopView不同，PopView在移除非最上层的view时，会将该view以上所有层都移除
--RemoveView可以移除栈中的任意一个view，不会影响其他view
function RemoveView(view)
	if view == _viewStack[#_viewStack] then
		--如果移除的时最上层界面，则和Pop逻辑一样
		PopTopView()
		return
	end
	local index = -1
	for i=#_viewStack-1, 1,-1 do
		local theView = _viewStack[i]
		if theView == view then
			_DisposeView(view) 
			index = i
			break
		end
	end
	if index == -1 then
		--没找到要移除的view，直接关闭该界面
		_DisposeView(view)
		return
	end
	for i=index,#_viewStack-1 do
		_viewStack[i] = _viewStack[i+1]
	end
	viewStack[#viewStack] = nil
end

--弹回到指定界面
function PopToTarget(view)
	if view == null then
		return
	end
	if view == _viewStack[#_viewStack - 1] then
		PopTopView()
		return
	end

	--查询栈中是否存在该view
	local hasView = false
	for i=#_viewStack - 1, 1, -1 do
		local theView = _viewStack[i]
		if theView == view then
			hasView = true
			break
		end
	end
	if not hasView then 
		--目标view不在栈中，不执行
		return
	end

	--先将上层的非全屏界面移除
	local hasFinish = false
	for i=#_viewStack, 1, -1 do
		local theView = _viewStack[i]
		if theView == view then
			hasFinish = true
			break
		elseif theView.fullScreen then
			break
		else
			_DisposeView(theView)
			_viewStack[i] = nil
		end
	end

	if hasFinish then
		--如果将非全屏界面移除时，已经弹出到了目标界面，则结束
		return
	end

	--将topview到目标view之间的view都移除
	local topView = _viewStack[#_viewStack]
	_viewStack[#_viewStack] = nil
	for i=#_viewStack, 1, -1 do
		local theView = _viewStack[i]
		if theView == view then
			_viewStack[i+1] = topView
			break
		else
			_DisposeView(theView)
			_viewStack[i] = nil
		end
	end

	--然后pop topview
	PopTopView()
end

--一直弹回到主界面
function PopToHome()
	local view = _viewStack[1]
	PopToTarget(view)
end

--android按返回键时可调用此函数
function DoAndroidBack()
	if isAnimate then
		return
	end
	if #_viewStack > 1 then
		PopTopView()
	else
		--todo 抛出事件，弹退出询问框
	end
end

--使用此方法判断该界面是否在最上面
function isTop(viewLogic)
	return _topView == viewLogic
end

