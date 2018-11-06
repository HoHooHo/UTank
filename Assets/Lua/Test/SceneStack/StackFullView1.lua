StackFullView1 = class("StackFullView1", ViewBaseLogic)

function StackFullView1:Awake(  )
	self._animator = self._gameObject:GetComponent(typeof(UnityEngine.Animator))
end

function StackFullView1:Start(  )
	self._luaBehaviour:AddClick("FullButton1", self.OnFullButton1Click)
	self._luaBehaviour:AddClick("DialogButton1", self.OnDialogButton1Click)
	self._luaBehaviour:AddClick("DialogButton2", self.OnDialogButton2Click)
	self._luaBehaviour:AddClick("CloseButton", self.OnCloseButtonClick)

	
end

function StackFullView1:OnFullButton1Click( btn )
	--self._animator:Play("FullViewInAnim")
	UIManager.Open("StackFullView2", nil, nil, nil, "scenestack")
end

function StackFullView1:OnDialogButton1Click( btn )
	--self._animator:Play("FullViewOutAnim")
	UIManager.Open("PopView1", nil, nil, nil, "scenestack")
end

function StackFullView1:OnDialogButton2Click( btn )
	--self._animator:Play("FullViewOutAnim")
	UIManager.Open("PopView2", nil, nil, nil, "scenestack")
end

function StackFullView1:OnCloseButtonClick( btn )
	self:Close()
end

--在界面显示的时候调用
--第一次被创建时会调用
--上层遮盖该界面的全屏界面被移除，漏出此界面时也会调用
function StackFullView1:OnEnable( )
	ulog("========== StackFullView1:OnEnable ==========")
end

--在界面不被显示的时候调用
--移除时会调用
--被其他全屏界面遮盖时会调用
function StackFullView1:OnDisable( )
	ulog("========== StackFullView1:OnDisable ==========")
end

--第一次被创建时会调用
--变成最上层时会被调用
function StackFullView1:OnBeTop()
	ulog("========== StackFullView1:OnBeTop ==========")
end

--移除时会调用
--被任意界面(包括全屏和非全屏)遮盖时会调用
function StackFullView1:OnBeNotTop()
	ulog("========== StackFullView1:OnBeNotTop ==========")
end

--入场动画播放完毕后调用, isFirst表示第一次显示
function StackFullView1:OnDidShow(isFirst)
	ulog("========== StackFullView1:OnDidShow ========== ")
	ulog(isFirst)
end

function StackFullView1:PlayInAnim(prevView, newView, onComplete)
	self._inAnimComplete = onComplete
	self._animator:Play("FullViewInAnim")
	--TweenHelper.TweenAlpha(newView._gameObject, 0, 1, 0.4, onComplete)
end

function StackFullView1:PlayOutAnim(prevView, newView, onComplete)
	-- TweenHelper.TweenAlpha(prevView._gameObject, 1, 0, 0.4, onComplete)
	self._outAnimComplete = onComplete
	self._animator:Play("FullViewOutAnim")
end

function StackFullView1:OnAnimationEvent(eventName)
	ulog("========== StackFullView1:OnAnimationEvent ==========")
	ulog(eventName)
	if eventName == "InAnimEnd" and self._inAnimComplete then
		self._inAnimComplete()
		self._inAnimComplete = nil
	elseif eventName== "OutAnimEnd" and self._outAnimComplete then
		self._outAnimComplete()
		self._outAnimComplete = nil
	end
end

--要被其他全屏界面盖住时，播放的动画
function StackFullView1.BeCoverAnim(prevView, newView, onComplete)
	StackFullView1.PlayOutAnim(prevView, prevView, newView, onComplete)
end

--要被其他全屏界面盖住时，播放的动画
function StackFullView1.BeTopAnim(prevView, newView, onComplete)
	StackFullView1.PlayInAnim(newView, prevView, newView, onComplete)
end

function StackFullView1.PushAnim(prevView, newView, onComplete)
	StackFullView1.PlayInAnim(newView, prevView, newView, onComplete)
end

function StackFullView1.PopAnim(prevView, newView, onComplete)
	StackFullView1.PlayOutAnim(prevView, prevView, newView, onComplete)
end