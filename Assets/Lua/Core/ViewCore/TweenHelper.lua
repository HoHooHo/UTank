module("TweenHelper", package.seeall)

function TweenAlpha(gameObject,startValue, endValue, duration, onComplete)
	local canvasGroup = gameObject:GetComponent(typeof(UnityEngine.CanvasGroup))
	if not canvasGroup then
		canvasGroup = gameObject:AddComponent(typeof(UnityEngine.CanvasGroup))
	end
	local OnUpdate = function(value)
		canvasGroup.alpha = value
	end
	canvasGroup.alpha = startValue
	local tween = TweenUtils.TweenFloat(startValue, endValue, duration, OnUpdate)
	if onComplete then
		TweenUtils.OnComplete(tween, onComplete)
	end
	return tween
end

function SetAlpha(gameObject, value)
	local canvasGroup = gameObject:GetComponent(typeof(UnityEngine.CanvasGroup))
	if not canvasGroup then
		canvasGroup = gameObject:AddComponent(typeof(UnityEngine.CanvasGroup))
	end
	canvasGroup.alpha = value
end

function TransitionNone(prevView, newView, onComplete)
	if onComplete then
		onComplete()
	end
end

--默认的全屏界面进入动画
function TransitionFadeIn(prevView, newView, onComplete)
	TweenAlpha(newView._gameObject, 0, 1, 0.4, onComplete)
end

--默认的全屏界面关闭动画
function TransitionFadeOut(prevView, newView, onComplete)
	SetAlpha(newView._gameObject, 1)
	TweenAlpha(prevView._gameObject, 1, 0, 0.4, onComplete)
end

--默认的弹框打开动画
function TransitionPopShow(prevView, newView, onComplete)
	local transform = newView._transform
	transform.localScale = Vector3.zero
	local tween = transform:DOScale(1, 0.3):SetEase(DG.Tweening.Ease.IntToEnum(27))
	if onComplete then
		TweenUtils.OnComplete(tween, onComplete)
	end
end

--默认的弹框关闭动画
function TransitionPopClose(prevView, newView, onComplete)
	local transform = prevView._transform
	local tween = transform:DOScale(0, 0.3):SetEase(DG.Tweening.Ease.IntToEnum(26))
	if onComplete then
		TweenUtils.OnComplete(tween, onComplete)
	end
end