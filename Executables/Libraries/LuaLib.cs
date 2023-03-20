using Godot;
using System;
using NLua;

// This class is responsible for managing custom binding libraries and is meant to
// be inherited by library classes
public partial class LuaLib : Resource
{
    public string name;
    public LuaTable table;

    public LuaLib(string name)
    {
        this.name = name;
        this.table = PrepTable();
    }

    // Used to register this library with a set of globals
    public virtual Lua Register(Lua state)
    {
        state[name] = PrepTable();
        return state;
    }

    // This function preps a new lua table to be inserted into the state globals
    // This table will have the library functions added to it in overridden functions
    public virtual LuaTable PrepTable()
    {
        // Create a table object
        Lua tempState = new Lua();
        tempState.NewTable(name);
        return tempState.GetTable(name);
    }
}
