using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LuaInterface;
using System.IO;
using System;

public class LuaLoader : MonoBehaviour {

	private static LuaState m_luaState = null;

	public static LuaState luaState {
		get{
			return m_luaState;
		}
	}



	private LuaLooper m_looper = null;

	// Use this for initialization
	void Awake () {
		Debug.Log ("Application.streamingAssetsPath = " + Application.streamingAssetsPath);
		Debug.Log ("Application.persistentDataPath = " + Application.persistentDataPath);


		m_luaState = new LuaState ();

		m_luaState.AddSearchPath(LuaConst.luaResDir + "/ToLua/");
		m_luaState.AddSearchPath(LuaConst.luaStreamingDir);
		m_luaState.AddSearchPath(LuaConst.luaStreamingDir + "/ToLua/");

		m_luaState.Start ();
		LuaBinder.Bind (m_luaState);
		m_looper = gameObject.AddComponent<LuaLooper> ();
		m_looper.luaState = m_luaState;

		OpenLibs ();
		m_luaState.LuaSetTop(0);

		LuaHelperCS.Instance.Init (gameObject);

		List<string> searchPaths = LuaFileUtils.Instance.GetSearchPaths ();

		for (int i = 0; i < searchPaths.Count; i++) {
			Debug.Log ("SearchPath: " + i + " ====>>>> " + searchPaths[i]);
		}


		m_luaState.DoFile("Main");
	}
	
	// Update is called once per frame
	void Update () {
	}

	void Destroy(){

		LuaHelperCS.Instance.Uninit ();

		m_looper.Destroy ();

		m_luaState.Dispose();
		m_luaState = null;
	}



	//////////////////////////////////////

	protected virtual void OpenLibs()
	{
		m_luaState.OpenLibs(LuaDLL.luaopen_pb);
		m_luaState.OpenLibs(LuaDLL.luaopen_struct);
		m_luaState.OpenLibs(LuaDLL.luaopen_lpeg);
		#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
		m_luaState.OpenLibs(LuaDLL.luaopen_bit);
		#endif

		if (LuaConst.openLuaSocket)
		{
			OpenLuaSocket();            
		}        

		if (LuaConst.openZbsDebugger)
		{
			OpenZbsDebugger();
		}
	}

	public void OpenZbsDebugger(string ip = "localhost")
	{
		if (!Directory.Exists(LuaConst.zbsDir))
		{
			Debugger.LogWarning("ZeroBraneStudio not install or LuaConst.zbsDir not right");
			return;
		}

		if (!LuaConst.openLuaSocket)
		{                            
			OpenLuaSocket();
		}

		if (!string.IsNullOrEmpty(LuaConst.zbsDir))
		{
			m_luaState.AddSearchPath(LuaConst.zbsDir);
		}

		m_luaState.LuaDoString(string.Format("DebugServerIp = '{0}'", ip));
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LuaOpen_Socket_Core(IntPtr L)
	{        
		return LuaDLL.luaopen_socket_core(L);
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LuaOpen_Mime_Core(IntPtr L)
	{
		return LuaDLL.luaopen_mime_core(L);
	}

	protected void OpenLuaSocket()
	{
		LuaConst.openLuaSocket = true;

		m_luaState.BeginPreLoad();
		m_luaState.RegFunction("socket.core", LuaOpen_Socket_Core);
		m_luaState.RegFunction("mime.core", LuaOpen_Mime_Core);                
		m_luaState.EndPreLoad();                     
	}

	//cjson 比较特殊，只new了一个table，没有注册库，这里注册一下
	protected void OpenCJson()
	{
		m_luaState.LuaGetField(LuaIndexes.LUA_REGISTRYINDEX, "_LOADED");
		m_luaState.OpenLibs(LuaDLL.luaopen_cjson);
		m_luaState.LuaSetField(-2, "cjson");

		m_luaState.OpenLibs(LuaDLL.luaopen_cjson_safe);
		m_luaState.LuaSetField(-2, "cjson.safe");                               
	}
}
