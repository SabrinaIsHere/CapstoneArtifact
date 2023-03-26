using Godot;
using System.Collections.Generic;
using NLua;

public partial class LuaEvent : LuaLib
{
    GodotObject eventHandler;

    public LuaEvent(LuaGlobals globals, GodotObject eventHandler) : base("event", globals)
    {
        this.eventHandler = eventHandler;
    }

    public override LuaTable PrepTable(Lua state)
    {
        LuaTable tempTable = base.PrepTable(state);
        tempTable["trigger"] = trigger;
        return tempTable;
    }

    public void trigger(string evnName, string evnCategory, LuaTable args)
    {
        List<string> tempArgs = new List<string>();
        foreach (var i in args.Values)
        {
            tempArgs.Add(i.ToString());
        }
        eventHandler.Call("trigger", evnName, evnCategory, tempArgs.ToArray());
    }
}
