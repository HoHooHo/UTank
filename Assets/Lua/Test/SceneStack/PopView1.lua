PopView1 = class("PopView1", ViewBaseLogic)
PopView1.layer = SceneHelper.LAYER_TYPE.DIALOG_LAYER

function PopView1:Start(  )
	self._luaBehaviour:AddClick("FullButton1", self.OnFullButton1Click)
	self._luaBehaviour:AddClick("DialogButton1", self.OnDialogButton1Click)
end

function PopView1:OnFullButton1Click( btn )
	UIManager.Open("StackFullView2", nil, nil, nil, "scenestack")
end

function PopView1:OnDialogButton1Click( btn )
	UIManager.Open("PopView1", nil, nil, nil, "scenestack")
end