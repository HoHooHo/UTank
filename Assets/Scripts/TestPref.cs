using UnityEngine;
using System.Collections;

public class TestPref : MonoBehaviour {

	/*
	//不同平台下StreamingAssets的路径是不同的，这里需要注意一下。
	public static readonly string PathURL =
		#if UNITY_ANDROID
		"jar:file://" + Application.dataPath + "!/assets/";
		#elif UNITY_IPHONE
		Application.dataPath + "/Raw/";
		#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
		"file://" + Application.dataPath + "/StreamingAssets/";
		#else
		string.Empty;
		#endif
	*/

	//string PathURL = "file://" + Application.streamingAssetsPath + "/";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}



	void OnGUI(){
		if (GUI.Button (new Rect (550, 250, 100, 50), "testResManager")) {
			LuaHelperCS.Instance.ResManager.LoadPrefab ("assetbundle", "AssetBundle", delegate(UnityEngine.Object[] objs) {

				Instantiate(objs[0]);
			});
		}
	}
}
