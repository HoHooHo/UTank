using LuaInterface;
using System;
using UnityEngine;

[AddComponentMenu("UI/Custom/ListItem")]
public class ListItem : LuaBehaviour
{
    LuaFunction luaOnRefresh;
	public ListItem ()
	{
	}

    override protected void Awake()
    {
        base.Awake();
        luaOnRefresh = lua.GetFunction(luaFile + ".OnRefreshData");
    }

	public void RefreshWithData(object tableData, int index){
        this.OnRefreshData(tableData, index);
	}

    virtual protected void OnRefreshData(object tableData, int index)
    {
        if (luaOnRefresh != null)
        {
            luaOnRefresh.BeginPCall();
            luaOnRefresh.Push(cls);
            luaOnRefresh.Push(tableData);
            luaOnRefresh.Push(index);
            luaOnRefresh.PCall();
            luaOnRefresh.EndPCall();
        }      	
	}

    override  protected void DisposeLuaFunction()
    {
        base.DisposeLuaFunction();
        if (luaOnRefresh != null)
        {
            luaOnRefresh.Dispose();
            luaOnRefresh = null;
        }
    }
}


