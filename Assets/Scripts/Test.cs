using UnityEngine;
using System.Collections;

public class Test : MonoBehaviour {

	private GameObject go;

	// Use this for initialization
	void Start () {
		go = GameObject.Find ("Lua2");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI(){
		if (GUI.Button (new Rect (100, 100, 100, 50), "destroy lua1")) {
			Debug.Log ("*-*******************************");

			GameObject go = GameObject.Find ("Lua1");
			Destroy (go);
		}

		if (GUI.Button (new Rect (250, 100, 100, 50), "active lua2")) {
			Debug.Log ("*-*******************************");

			go.SetActive (!go.activeSelf);
		}
	}
}
