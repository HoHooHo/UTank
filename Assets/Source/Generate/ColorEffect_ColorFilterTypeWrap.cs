﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class ColorEffect_ColorFilterTypeWrap
{
	public static void Register(LuaState L)
	{
		L.BeginEnum(typeof(ColorEffect.ColorFilterType));
		L.RegVar("None", get_None, null);
		L.RegVar("Invert", get_Invert, null);
		L.RegVar("AdJust", get_AdJust, null);
		L.RegVar("Tint", get_Tint, null);
		L.RegVar("Tint2", get_Tint2, null);
		L.RegFunction("IntToEnum", IntToEnum);
		L.EndEnum();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_None(IntPtr L)
	{
		ToLua.Push(L, ColorEffect.ColorFilterType.None);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Invert(IntPtr L)
	{
		ToLua.Push(L, ColorEffect.ColorFilterType.Invert);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_AdJust(IntPtr L)
	{
		ToLua.Push(L, ColorEffect.ColorFilterType.AdJust);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Tint(IntPtr L)
	{
		ToLua.Push(L, ColorEffect.ColorFilterType.Tint);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int get_Tint2(IntPtr L)
	{
		ToLua.Push(L, ColorEffect.ColorFilterType.Tint2);
		return 1;
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int IntToEnum(IntPtr L)
	{
		int arg0 = (int)LuaDLL.lua_tonumber(L, 1);
		ColorEffect.ColorFilterType o = (ColorEffect.ColorFilterType)arg0;
		ToLua.Push(L, o);
		return 1;
	}
}

