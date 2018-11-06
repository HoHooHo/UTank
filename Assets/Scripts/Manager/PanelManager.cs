using UnityEngine;
using LuaInterface;

public class PanelManager : MonoBehaviour {

	/*
	public void CreatePanel(string name, LuaFunction func = null){
		string assetName = name.ToLower ();
		string abName = name.ToLower ();

		ResManager.LoadPrefab (abName, assetName, delegate(UnityEngine.Object[] objs) {
			if(objs.Length == 0) return;

			GameObject prefab = objs[0] as GameObject;
			//if(Parent.FindChild(name) != null || prefab == null) return;

			GameObject go = Instantiate(prefab) as GameObject;
			go.name = assetName;
			go.layer = LayerMask.NameToLayer("Default");
			//go.transform.SetParent(parent);

		});
	}*/
}
