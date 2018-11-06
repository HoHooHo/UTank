ImageEffectTest = class("ImageEffectTest", ViewBaseLogic)

function ImageEffectTest:Start(  )
	--置灰
	local image4 = self._transform:Find("Image4").gameObject
	local image4Effect = image4:AddComponent(typeof(ColorEffect))
	image4Effect.gray = true

	--混合模式
	local image5 = self._transform:Find("Image5").gameObject
	local image5Effect = image5:AddComponent(typeof(ColorEffect))
	image5Effect.blendType = ColorEffect.BlendType.IntToEnum(2)

	--反色
	local image6 = self._transform:Find("Image6").gameObject
	local image6Effect = image6:AddComponent(typeof(ColorEffect))
	image6Effect.colorFilterType = ColorEffect.ColorFilterType.IntToEnum(2)
	local OnUpdate = function(value)
		image6Effect.brightness = value
	end
	local OnComplete = function()
		ulog('OnTweenComplete')
	end
	self.tween1 = TweenUtils.TweenFloat(0, 1, 3, OnUpdate)
	TweenUtils.OnComplete(self.tween1, OnComplete)

	self.tween2 = image4.transform:DOMoveX(image4.transform.localPosition.x + 100, 1, false)
	 	:SetEase(DG.Tweening.Ease.IntToEnum(9)):SetLoops(-1, DG.Tweening.LoopType.IntToEnum(1))
end

function ImageEffectTest:OnDestroy(  )
	if self.tween1 then
		self.tween1:Kill(false)
		self.tween1 = nil
	end
	if self.tween2 then
		self.tween2:Kill(false)
		self.tween2 = nil
	end
end


