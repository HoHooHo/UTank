using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasHelper : MonoBehaviour {

	void Awake(){
		CanvasScaler cs = GetComponent<CanvasScaler> ();
		cs.matchWidthOrHeight = (float)Screen.width / (float)Screen.height >= (float)cs.referenceResolution.x / (float)cs.referenceResolution.y ? 1 : 0;
	}

	void Start () {
		LuaLoader.luaState ["Win.Scale"] = gameObject.transform.localScale;
		LuaLoader.luaState ["Win.Size.width"] = ((RectTransform)gameObject.transform).sizeDelta.x;
		LuaLoader.luaState ["Win.Size.height"] = ((RectTransform)gameObject.transform).sizeDelta.y;
	}
}
