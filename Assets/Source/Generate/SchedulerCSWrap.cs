﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class SchedulerCSWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(SchedulerCS), typeof(UnityEngine.MonoBehaviour));
		L.RegFunction("Schedule", Schedule);
		L.RegFunction("ScheduleOnce", ScheduleOnce);
		L.RegFunction("Unschedule", Unschedule);
		L.RegFunction("UnscheduleByTarget", UnscheduleByTarget);
		L.RegFunction("UnscheduleAll", UnscheduleAll);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Schedule(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 6);
			SchedulerCS obj = (SchedulerCS)ToLua.CheckObject(L, 1, typeof(SchedulerCS));
			LuaFunction arg0 = ToLua.CheckLuaFunction(L, 2);
			UnityEngine.GameObject arg1 = (UnityEngine.GameObject)ToLua.CheckUnityObject(L, 3, typeof(UnityEngine.GameObject));
			float arg2 = (float)LuaDLL.luaL_checknumber(L, 4);
			int arg3 = (int)LuaDLL.luaL_checknumber(L, 5);
			float arg4 = (float)LuaDLL.luaL_checknumber(L, 6);
			SchedulerInfo o = obj.Schedule(arg0, arg1, arg2, arg3, arg4);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ScheduleOnce(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 4);
			SchedulerCS obj = (SchedulerCS)ToLua.CheckObject(L, 1, typeof(SchedulerCS));
			LuaFunction arg0 = ToLua.CheckLuaFunction(L, 2);
			UnityEngine.GameObject arg1 = (UnityEngine.GameObject)ToLua.CheckUnityObject(L, 3, typeof(UnityEngine.GameObject));
			float arg2 = (float)LuaDLL.luaL_checknumber(L, 4);
			SchedulerInfo o = obj.ScheduleOnce(arg0, arg1, arg2);
			ToLua.PushObject(L, o);
			return 1;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int Unschedule(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 3);
			SchedulerCS obj = (SchedulerCS)ToLua.CheckObject(L, 1, typeof(SchedulerCS));
			SchedulerInfo arg0 = (SchedulerInfo)ToLua.CheckObject(L, 2, typeof(SchedulerInfo));
			UnityEngine.GameObject arg1 = (UnityEngine.GameObject)ToLua.CheckUnityObject(L, 3, typeof(UnityEngine.GameObject));
			obj.Unschedule(arg0, arg1);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnscheduleByTarget(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 2);
			SchedulerCS obj = (SchedulerCS)ToLua.CheckObject(L, 1, typeof(SchedulerCS));
			UnityEngine.GameObject arg0 = (UnityEngine.GameObject)ToLua.CheckUnityObject(L, 2, typeof(UnityEngine.GameObject));
			obj.UnscheduleByTarget(arg0);
			return 0;
		}
		catch(Exception e)
		{
			return LuaDLL.toluaL_exception(L, e);
		}
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int UnscheduleAll(IntPtr L)
	{
		try
		{
			ToLua.CheckArgsCount(L, 1);
			SchedulerCS obj = (SchedulerCS)ToLua.CheckObject(L, 1, typeof(SchedulerCS));
			obj.UnscheduleAll();
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

