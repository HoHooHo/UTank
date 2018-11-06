PopView2 = class("PopView2", ViewBaseLogic)
PopView2.layer = SceneHelper.LAYER_TYPE.DIALOG_LAYER

function PopView2:Awake(  )
	self._animator = self._gameObject:GetComponent(typeof(UnityEngine.Animator))
end

function PopView2:Start(  )
	self._luaBehaviour:AddClick("FullButton1", self.OnFullButton1Click)
	self._luaBehaviour:AddClick("DialogButton1", self.OnDialogButton1Click)
end

function PopView2:OnFullButton1Click( btn )
	UIManager.Open("StackFullView2", nil, nil, nil, "scenestack")
end

function PopView2:OnDialogButton1Click( btn )
	UIManager.Open("PopView2", nil, nil, nil, "scenestack")
	--self._animator:Play("PopViewOutAnim")
end

function PopView2.PushAnim(prevView, newView, onComplete)
	PopView2.PlayInAnim(newView, prevView, newView, onComplete)
end

function PopView2.PopAnim(prevView, newView, onComplete)
	PopView2.PlayOutAnim(prevView, prevView, newView, onComplete)
end

function PopView2:PlayInAnim(prevView, newView, onComplete)
	self._inAnimComplete = onComplete
	self._animator:Play("PopViewInAnim")
end

function PopView2:PlayOutAnim(prevView, newView, onComplete)
	self._outAnimComplete = onComplete
	self._animator:Play("PopViewOutAnim")
end

function PopView2:OnAnimationEvent(eventName)
	ulog('eventName')
	ulog(eventName)
	if eventName == "InAnimEnd" and self._inAnimComplete then
		self._inAnimComplete()
		self._inAnimComplete = nil
	elseif eventName== "OutAnimEnd" and self._outAnimComplete then

		self._outAnimComplete()
		self._outAnimComplete = nil
	end
end