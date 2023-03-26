using Godot;
using System;
using NLua;

// This class is responsible for managing custom binding libraries and is meant to
// be inherited by library classes
public partial class LuaLib : Resource
{
    public string name;
    public Lua state;
    public LuaGlobals globals;

    public LuaLib(string name, LuaGlobals globals)
    {
        this.name = name;
        this.state = globals.state;
        this.globals = globals;
    }

    // Used to register this library with a set of globals
    public virtual Lua Register(Lua state)
    {
        PrepTable(state);
        return state;
    }

    // This function preps a new lua table to be inserted into the state globals
    // This table will have the library functions added to it in overridden functions
    public virtual LuaTable PrepTable(Lua state)
    {
        // Create a table object
        state.NewTable(name);
        return state.GetTable(name);
    }
}
