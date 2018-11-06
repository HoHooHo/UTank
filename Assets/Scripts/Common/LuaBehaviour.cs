using UnityEngine;
using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LuaBehaviour : MonoBehaviour {

	public string luaFile = "";
    public bool fullScreen = true;
    public AnimationCurve ac;
	protected LuaTable cls;

    protected LuaState lua;
	LuaFunction luaAwake;
	LuaFunction luaOnEnable;
	LuaFunction luaStart;
	LuaFunction luaFixedUpdate;
	LuaFunction luaUpdate;
	LuaFunction luaLateUpdate;
	LuaFunction luaOnGUI;
	LuaFunction luaOnDisable;
	LuaFunction luaOnDestroy;

	Dictionary<GameObject, LuaFunction> luaOnClicks = new Dictionary<GameObject, LuaFunction>();



	public LuaTable Lua{
		get{
			return cls;
		}
	}

	private void CallLua(LuaFunction fun){
		if (fun != null) {
			fun.BeginPCall ();
			fun.Push(cls);
			fun.PCall ();
			fun.EndPCall ();
			fun.Dispose ();
		}
	}

	virtual protected void Awake()
	{
		lua = LuaLoader.luaState;

		LuaFunction luaNew = lua.GetFunction (luaFile + ".New");
		luaNew.BeginPCall ();
		luaNew.PCall ();
		cls = luaNew.CheckLuaTable ();
		luaNew.EndPCall ();
		luaNew.Dispose ();

		cls ["_gameObject"] = gameObject;
		cls ["_transform"] = gameObject.transform;
		cls ["_luaBehaviour"] = gameObject.GetComponent<LuaBehaviour>();
        cls["fullScreen"] = fullScreen;

		luaAwake = cls.GetLuaFunction ("IAwake");
		luaOnEnable = cls.GetLuaFunction ("IOnEnable");
		luaStart = cls.GetLuaFunction ("IStart");
		luaFixedUpdate = cls.GetLuaFunction ("FixedUpdate");
		luaUpdate = cls.GetLuaFunction ("Update");
		luaLateUpdate = cls.GetLuaFunction ("LateUpdate");
		luaOnGUI = cls.GetLuaFunction ("OnGUI");
		luaOnDisable = cls.GetLuaFunction ("IOnDisable");
		luaOnDestroy = cls.GetLuaFunction ("IOnDestroy");

		CallLua (luaAwake);
		luaAwake = null;
	}

	void OnEnable(){
		if (luaOnEnable != null) {
			luaOnEnable.BeginPCall ();
			luaOnEnable.Push (cls);
			luaOnEnable.PCall ();
			luaOnEnable.EndPCall ();
		}
	}

	void Start () {
		CallLua (luaStart);
		luaStart = null;
	}

	void FixedUpdate () {
		if (luaFixedUpdate != null) {
			luaFixedUpdate.BeginPCall ();
			luaFixedUpdate.Push(cls);
			luaFixedUpdate.PCall ();
			luaFixedUpdate.EndPCall ();
		}
	}

	void Update () {
		if (luaUpdate != null) {
			luaUpdate.BeginPCall ();
			luaUpdate.Push(cls);
			luaUpdate.PCall ();
			luaUpdate.EndPCall ();
		}
	}

	void LateUpdate () {
		if (luaLateUpdate != null) {
			luaLateUpdate.BeginPCall ();
			luaLateUpdate.Push(cls);
			luaLateUpdate.PCall ();
			luaLateUpdate.EndPCall ();
		}
	}

	void OnGUI(){
		if (luaOnGUI != null) {
			luaOnGUI.BeginPCall ();
			luaOnGUI.Push(cls);
			luaOnGUI.PCall ();
			luaOnGUI.EndPCall ();
		}
	}

	void OnDisable(){
		if (luaOnDisable != null) {
			luaOnDisable.BeginPCall ();
			luaOnDisable.Push(cls);
			luaOnDisable.PCall ();
			luaOnDisable.EndPCall ();
		}
	}

	void OnDestroy(){

		foreach (var dic in luaOnClicks) {
			if (dic.Value != null) {
				dic.Value.Dispose ();
			}
		}
		luaOnClicks.Clear ();

        DisposeLuaFunction();

		CallLua (luaOnDestroy);
		luaOnDestroy = null;

		cls.Dispose ();
		cls = null;
	}

    void OnAnimationEvent(string eventName)
    {
        LuaFunction onAnimationEvent = cls.GetLuaFunction("OnAnimationEvent");
        if (onAnimationEvent == null) {
            return;
        }
        onAnimationEvent.BeginPCall();
        onAnimationEvent.Push(cls);
        onAnimationEvent.Push(eventName);
        onAnimationEvent.PCall();
        onAnimationEvent.EndPCall();
        onAnimationEvent.Dispose();
        onAnimationEvent = null;
    }

    protected virtual void DisposeLuaFunction()
    {
        if (luaOnEnable != null) {
			luaOnEnable.Dispose ();
			luaOnEnable = null;
		}

		if (luaFixedUpdate != null) {
			luaFixedUpdate.Dispose ();
			luaFixedUpdate = null;
		}

		if (luaUpdate != null) {
			luaUpdate.Dispose ();
			luaUpdate = null;
		}

		if (luaLateUpdate != null) {
			luaLateUpdate.Dispose ();
			luaLateUpdate = null;
		}

		if (luaOnGUI != null) {
			luaOnGUI.Dispose ();
			luaOnGUI = null;
		}

		if (luaOnDisable != null) {
			luaOnDisable.Dispose ();
			luaOnDisable = null;
		}
    }

	/// the click event.
	public void OnClick(GameObject go){
		LuaFunction cb = null;
		if (!luaOnClicks.TryGetValue (go, out cb)) {
			cb = LuaLoader.luaState.GetFunction (luaFile + ".On" + go.name + "Click");
			luaOnClicks.Add (go, cb);
		}

		if (cb != null) {
			cb.BeginPCall ();
			cb.Push(cls);
			cb.Push (go);
			cb.PCall ();
			cb.EndPCall ();
		}
	}

	public void AddClick(string name, LuaFunction func){
		GameObject go = transform.Find (name).gameObject;
		if (go != null && func != null) {
			luaOnClicks.Add (go, func);

			go.GetComponent<Button> ().onClick.AddListener (delegate() {
				func.BeginPCall();
				func.Push(cls);
				func.Push(go);
				func.PCall();
				func.EndPCall();
			});
		}
	}

	public void AddClick(GameObject go, LuaFunction func){
		if (go != null && func != null) {
			luaOnClicks.Add (go, func);

			go.GetComponent<Button> ().onClick.AddListener (delegate() {
				func.BeginPCall();
				func.Push(cls);
				func.Push(go);
				func.PCall();
				func.EndPCall();
			});
		}
	}

	public void RemoveClick(string name){
		GameObject go = gameObject.transform.Find (name).gameObject;
		if (go != null) {
			LuaFunction func = null;

			if(luaOnClicks.TryGetValue(go, out func)){
				func.Dispose ();
				func = null;
				luaOnClicks.Remove (go);
				go.GetComponent<Button> ().onClick.RemoveAllListeners ();
			}
		}
	}

	public void RemoveClick(GameObject go){
		if (go != null) {
			LuaFunction func = null;

			if(luaOnClicks.TryGetValue(go, out func)){
				func.Dispose ();
				func = null;
				luaOnClicks.Remove (go);
				go.GetComponent<Button> ().onClick.RemoveAllListeners ();
			}
		}
	}
}
