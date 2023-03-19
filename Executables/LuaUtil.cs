using Godot;
using System;
using NLua;

public partial class LuaUtil : Resource
{
	private Lua state;

	public LuaUtil()
	{
        // New state
		state = new Lua();

        // Remove outside libraries
        state.DoString("_ENV = {}");

        // Load game library bindings
        state.RegisterFunction("print", this, this.GetType().GetMethod("LuaPrint"));
	}

	// This is very much temporary and for debugging
    public void LuaPrint(string val)
    {
        GD.Print(val);
    }

	public void ExecuteString(string exe)
	{
		GD.Print("ExecuteString called");
		state.DoString(exe);
	}

	public object[] ExecuteScript(string path)
	{
		GD.Print("Executing " + path + "...");
		return state.DoFile(path);
	}
}
