using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using LuaInterface;

[AddComponentMenu("UI/Custom/LangHelperCS")]
[RequireComponent(typeof(Text))]
public class LangHelperCS : MonoBehaviour {

	public static LuaFunction s_luaGetLang = null;

	void Awake(){
		Text label = gameObject.GetComponent<Text> ();

		if (s_luaGetLang == null) {
			s_luaGetLang = LuaLoader.luaState.GetFunction ("GetLang");
		}

		s_luaGetLang.BeginPCall ();
		s_luaGetLang.Push(label.text);
		s_luaGetLang.PCall ();

		label.text = s_luaGetLang.CheckString().Replace ("\\n", "\n");
		s_luaGetLang.EndPCall ();

	}
}
