﻿//this source code was auto-generated by tolua#, do not modify it
using System;
using LuaInterface;

public class UnityEngine_UI_BaseMeshEffectWrap
{
	public static void Register(LuaState L)
	{
		L.BeginClass(typeof(UnityEngine.UI.BaseMeshEffect), typeof(UnityEngine.EventSystems.UIBehaviour));
		L.RegFunction("ModifyMesh", ModifyMesh);
		L.RegFunction("__eq", op_Equality);
		L.RegFunction("__tostring", ToLua.op_ToString);
		L.EndClass();
	}

	[MonoPInvokeCallbackAttribute(typeof(LuaCSFunction))]
	static int ModifyMesh(IntPtr L)
	{
		try
		{
			int count = LuaDLL.lua_gettop(L);

			if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.UI.BaseMeshEffect), typeof(UnityEngine.UI.VertexHelper)))
			{
				UnityEngine.UI.BaseMeshEffect obj = (UnityEngine.UI.BaseMeshEffect)ToLua.ToObject(L, 1);
				UnityEngine.UI.VertexHelper arg0 = (UnityEngine.UI.VertexHelper)ToLua.ToObject(L, 2);
				obj.ModifyMesh(arg0);
				return 0;
			}
			else if (count == 2 && TypeChecker.CheckTypes(L, 1, typeof(UnityEngine.UI.BaseMeshEffect), typeof(UnityEngine.Mesh)))
			{
				UnityEngine.UI.BaseMeshEffect obj = (UnityEngine.UI.BaseMeshEffect)ToLua.ToObject(L, 1);
				UnityEngine.Mesh arg0 = (UnityEngine.Mesh)ToLua.ToObject(L, 2);
				obj.ModifyMesh(arg0);
				return 0;
			}
			else
			{
				return LuaDLL.luaL_throw(L, "invalid arguments to method: UnityEngine.UI.BaseMeshEffect.ModifyMesh");
			}
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

