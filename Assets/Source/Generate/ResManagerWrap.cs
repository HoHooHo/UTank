﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class ResManagerWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(ResManager), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("Init", Init);
		L.RegFunction("Retain", Retain);
		L.RegFunction("Release", Release);
		L.RegFunction("LoadPrefab", LoadPrefab);
		L.RegFunction("UnloadPrefab", UnloadPrefab);
		L.RegFunction("clear", clear);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Init(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			ResManager obj = (ResManager)ToLua.CheckObject(L, 1, typeof(ResManager));
			string arg0 = ToLua.CheckString(L, 2);
			System.Action arg1 = null;
			LuaTypes funcType3 = LuaDLL.lua_type(L, 3);

			if (funcType3 != LuaTypes.LUA_TFUNCTION)
			{
				 arg1 = (System.Action)ToLua.CheckObject(L, 3, typeof(System.Action));
			}
			else
			{
				LuaFunction func = ToLua.ToLuaFunction(L, 3);
				arg1 = DelegateFactory.CreateDelegate(typeof(System.Action), func) as System.Action;
			}

			LuaFunction arg2 = ToLua.CheckLuaFunction(L, 4);
			obj.Init(arg0, arg1, arg2);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Retain(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ResManager obj = (ResManager)ToLua.CheckObject(L, 1, typeof(ResManager));
			string arg0 = ToLua.CheckString(L, 2);
			bool o = obj.Retain(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Release(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ResManager obj = (ResManager)ToLua.CheckObject(L, 1, typeof(ResManager));
			string arg0 = ToLua.CheckString(L, 2);
			bool o = obj.Release(arg0);
			LuaDLL.lua_pushboolean(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int LoadPrefab(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(ResManager), typeof(string), typeof(string[]), typeof(LuaInterface.LuaFunction)))
			{
				ResManager obj = (ResManager)ToLua.ToObject(L, 1);
				string arg0 = ToLua.ToString(L, 2);
				string[] arg1 = ToLua.CheckStringArray(L, 3);
				LuaFunction arg2 = ToLua.ToLuaFunction(L, 4);
				obj.LoadPrefab(arg0, arg1, arg2);
				return 0;
			}
			else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(ResManager), typeof(string), typeof(string[]), typeof(System.Action<UnityEngine.Object[]>)))
			{
				ResManager obj = (ResManager)ToLua.ToObject(L, 1);
				string arg0 = ToLua.ToString(L, 2);
				string[] arg1 = ToLua.CheckStringArray(L, 3);
				System.Action<UnityEngine.Object[]> arg2 = null;
				LuaTypes funcType4 = LuaDLL.lua_type(L, 4);

				if (funcType4 != LuaTypes.LUA_TFUNCTION)
				{
					 arg2 = (System.Action<UnityEngine.Object[]>)ToLua.ToObject(L, 4);
				}
				else
				{
					LuaFunction func = ToLua.ToLuaFunction(L, 4);
					arg2 = DelegateFactory.CreateDelegate(typeof(System.Action<UnityEngine.Object[]>), func) as System.Action<UnityEngine.Object[]>;
				}

				obj.LoadPrefab(arg0, arg1, arg2);
				return 0;
			}
			else if (count == 4 && TypeChecker.CheckTypes(L, 1, typeof(ResManager), typeof(string), typeof(string), typeof(System.Action<UnityEngine.Object[]>)))
			{
				ResManager obj = (ResManager)ToLua.ToObject(L, 1);
				string arg0 = ToLua.ToString(L, 2);
				string arg1 = ToLua.ToString(L, 3);
				System.Action<UnityEngine.Object[]> arg2 = null;
				LuaTypes funcType4 = LuaDLL.lua_type(L, 4);

				if (funcType4 != LuaTypes.LUA_TFUNCTION)
				{
					 arg2 = (System.Action<UnityEngine.Object[]>)ToLua.ToObject(L, 4);
				}
				else
				{
					LuaFunction func = ToLua.ToLuaFunction(L, 4);
					arg2 = DelegateFactory.CreateDelegate(typeof(System.Action<UnityEngine.Object[]>), func) as System.Action<UnityEngine.Object[]>;
				}

				obj.LoadPrefab(arg0, arg1, arg2);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: ResManager.LoadPrefab");
			}
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnloadPrefab(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			ResManager obj = (ResManager)ToLua.CheckObject(L, 1, typeof(ResManager));
			string arg0 = ToLua.CheckString(L, 2);
			obj.UnloadPrefab(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int clear(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			ResManager obj = (ResManager)ToLua.CheckObject(L, 1, typeof(ResManager));
			obj.clear();
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
}
