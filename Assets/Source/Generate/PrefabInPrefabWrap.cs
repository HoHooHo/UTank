﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class PrefabInPrefabWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(PrefabInPrefab), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("ForceDrawDontEditablePrefab", ForceDrawDontEditablePrefab);
		L.RegFunction("DrawDontEditablePrefab", DrawDontEditablePrefab);
		L.RegFunction("GetPrefabFilePath", GetPrefabFilePath);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("Child", get_Child, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ForceDrawDontEditablePrefab(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			PrefabInPrefab obj = (PrefabInPrefab)ToLua.CheckObject(L, 1, typeof(PrefabInPrefab));
			obj.ForceDrawDontEditablePrefab();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int DrawDontEditablePrefab(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			PrefabInPrefab obj = (PrefabInPrefab)ToLua.CheckObject(L, 1, typeof(PrefabInPrefab));
			obj.DrawDontEditablePrefab();
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int GetPrefabFilePath(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			PrefabInPrefab obj = (PrefabInPrefab)ToLua.CheckObject(L, 1, typeof(PrefabInPrefab));
			string o = obj.GetPrefabFilePath();
			LuaDLL.lua_pushstring(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int op_Equality(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			UnityEngine.Object arg0 = (UnityEngine.Object)ToLua.ToObject(L, 1);
			UnityEngine.Object arg1 = (UnityEngine.Object)ToLua.ToObject(L, 2);
			bool o = arg0 == arg1;
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Child(IntPtr L)
	{
		object o = null;

		try
		{
			o = ToLua.ToObject(L, 1);
			PrefabInPrefab obj = (PrefabInPrefab)o;
			UnityEngine.GameObject ret = obj.Child;
			ToLua.Push(L, ret);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e, o == null ? "attempt to index Child on a nil value" : e.Message);
		}
	}
}

