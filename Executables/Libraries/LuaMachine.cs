using Godot;
using System;
using NLua;

public partial class LuaMachine : LuaLib
{
    GodotObject machine;

    public LuaMachine(LuaGlobals globals, GodotObject machine) : base ("machine", globals)
    {
        this.machine = machine;
    }

    public override LuaTable PrepTable(Lua state)
    {
        LuaTable tempTable = base.PrepTable(state);
        tempTable["execute"] = execute;
        return tempTable;
    }

    // Todo: be able to affect the machine name, etc.
    public string execute(string code)
    {
        return globals.Execute(code, true);
    }
}
