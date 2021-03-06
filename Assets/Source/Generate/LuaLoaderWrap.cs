﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class LuaLoaderWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(LuaLoader), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("OpenZbsDebugger", OpenZbsDebugger);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.RegVar("luaState", get_luaState, null);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int OpenZbsDebugger(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			LuaLoader obj = (LuaLoader)ToLua.CheckObject(L, 1, typeof(LuaLoader));
			string arg0 = ToLua.CheckString(L, 2);
			obj.OpenZbsDebugger(arg0);
			return 0;
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
	static int get_luaState(IntPtr L)
	{
		try
		{
			ToLua.PushObject(L, LuaLoader.luaState);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}
}

