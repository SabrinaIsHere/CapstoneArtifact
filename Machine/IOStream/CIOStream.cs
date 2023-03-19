using Godot;
using System;
using NLua;

public partial class CIOStream : Resource
{
    private Lua state;
    private LuaTable luaObject;

    public CIOStream()
    {
        // New state to run lua in
        state = new Lua();

        // New lua instance of this object
        /* Note: this object is meant to be passed to lua scripts for them to modify
           and define the methods 'PopEvent' and 'PushEvent'*/
        luaObject = InstanceTable();
    }

    public LuaTable InstanceTable()
    {
        state.NewTable("IOStream");
        LuaTable ret = state.GetTable("IOStream");
        return ret;
    }

    // These methods are called by the gdscript 'IOStream' object
    public string PopEvent(string newValue)
    {
        LuaFunction func = luaObject["PopEvent"] as LuaFunction;
        if (func != null)
        {
            var ret = (string) func.Call(newValue)[0] as string;
            if (ret != null && ret is string)
            {
                return ret;
            }
        }
        return null;
    }

    public string PushEvent(string givenValue)
    {
        LuaFunction func = luaObject["PushEvent"] as LuaFunction;
        if (func != null)
        {
            var ret = func.Call(givenValue)[0] as string;
            if (ret != null && ret is string)
            {
                return ret;
            }
        }
        return null;
    }
}
