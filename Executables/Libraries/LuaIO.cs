using System;
using NLua;
using Godot;

public partial class LuaIO : LuaLib
{
    CIOStream iostream;

    public LuaIO(CIOStream iostream) : base("io") 
    {
        this.iostream = iostream;
    }

    public override Lua Register(Lua state)
    {
        base.Register(state);
        // Makes 'print' a global function so you don't have to type 'io.print' every time
        state.RegisterFunction("print", this, this.GetType().GetMethod("print"));
        return state;
    }

    public override LuaTable PrepTable()
    {
        LuaTable tempTable = base.PrepTable();
        tempTable["print"] = this.GetType().GetMethod("print");
        return tempTable;
    }

    public void print(string value)
    {
        // Good for debugging sometimes
        //GD.Print(value);
        this.iostream.push(value);
    }
}
