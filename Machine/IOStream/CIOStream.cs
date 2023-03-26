using Godot;
using System;
using NLua;

public partial class CIOStream : Resource
{
    private Lua state;
    private LuaTable luaObject;
    public GodotObject gIOStream;

    public CIOStream()
    {
        // New state to run lua in
        state = new Lua();

        // New lua instance of this object
        /* Note: this object is meant to be passed to lua scripts for them to modify
           and define the methods 'PopEvent' and 'PushEvent'*/
        luaObject = InstanceTable(state);
    }

    // Get around godot being weird with parameters passed to constructors
    public void init(GodotObject gIOStream)
    {
        this.gIOStream = gIOStream;
    }

    // For convenience
    public void push(string value)
    {
        gIOStream.Call("push", value);
    }

    public string pop()
    {
        Variant ret_val = gIOStream.Call("pop");
        if (ret_val.AsString() != null)
        {
            return ret_val.AsString();
        } else
        {
            return "";
        }
    }

    public LuaTable InstanceTable(Lua state)
    {
        state.NewTable("IOStream");
        state.DoString(@"IOStream = {
            push_event = function(value) end,
            pop_event = function(value) end}");
        LuaTable ret = state.GetTable("IOStream");
        state["IOStream"] = null;
        return ret;
    }

    // These methods are called by the gdscript 'IOStream' object
    public string PopEvent(string newValue)
    {
        LuaFunction func = luaObject["pop_event"] as LuaFunction;
        if (func != null)
        {
            Object[] ret;
            try {
                ret = func.Call(newValue);
            } catch (Exception e)
            {
                GD.Print(e.ToString());
                return e.ToString();
            }
            if (ret != null && ret.Length > 0 && ret[0] is string)
            {
                return (string) ret[0];
            }
        }
        return null;
    }

    public string PushEvent(string givenValue)
    {
        LuaFunction func = luaObject["push_event"] as LuaFunction;
        if (func != null)
        {
            Object[] ret;
            try {
                ret = func.Call(givenValue);
            } catch (Exception e)
            {
                GD.Print(e.ToString());
                return e.ToString();
            }
            if (ret != null && ret.Length > 0 && ret[0] is string)
            {
                return (string) ret[0];
            }
        }
        return null;
    }
}
