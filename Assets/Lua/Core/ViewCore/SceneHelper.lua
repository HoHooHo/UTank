module("SceneHelper", package.seeall)

local _root
local TAG_START = 0
local _layerMap
local _noClickPanel = nil  --一个不能点击的层，让用户暂时不能点击界面
local _touchable = true

local function getNext(  )
    TAG_START = TAG_START + 1
    return TAG_START
end

LAYER_TYPE =
{
    BASE_LAYER          = getNext(),
    DIALOG_LAYER        = getNext(),
    GUIDE_LAYER         = getNext(),
    LOADING_LAYER       = getNext(),
    NET_LOADING_LAYER   = getNext(),
    DEBUG_LAYER			= getNext()
}

function Init( root )
	_root = root

	_layerMap = {}
	for name,index in pairs(LAYER_TYPE) do
		local transform = CreateFullScreenPanel(name, _root)
		_layerMap[index] = transform
        transform:SetParent(_root, false)
    end

    for index,layerTransform in pairs(_layerMap) do
        layerTransform:SetSiblingIndex(index)
    end
end

function CreateEmptyUI(name)
	local layer = UnityEngine.GameObject.New()
    layer.name = name
    layer.layer = 5
	local transform = layer:AddComponent(typeof(UnityEngine.RectTransform))
    return transform
end

function CreateFullScreenPanel(name)
	local transform = CreateEmptyUI(name)
	transform.anchorMin = _root.anchorMin
    transform.anchorMax = _root.anchorMax
    transform.sizeDelta = _root.sizeDelta
    return transform
end

function AddChild(childTransform, type)
	local layerTransform = _layerMap[type]
	if not layerTransform then
		return
	end
	childTransform:SetParent(layerTransform, false)
end

--设置用户能不能点击界面(debug层不受影响)
function SetTouchable(touchable)
	if _touchable == touchable then
		return
	end
	_touchable = touchable
	if not touchable then
		if not _noClickPanel then
			local transform = SceneHelper.CreateFullScreenPanel("noclick")

			SceneHelper.AddChild(transform, SceneHelper.LAYER_TYPE.LOADING_LAYER)
			_noClickPanel = transform.gameObject
			_noClickPanel:AddComponent(typeof(Panel))
		end
		_noClickPanel:SetActive(true)
	else
		if 	_noClickPanel then
			_noClickPanel:SetActive(false)
		end
	end
end

