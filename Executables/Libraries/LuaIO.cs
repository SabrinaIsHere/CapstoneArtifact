using System;
using NLua;
using Godot;

// Handles interactions with input and output, namely the Terminal and IOStreams
public partial class LuaIO : LuaLib
{
    CIOStream iostream;
    GodotObject terminal;
    Lua state;

    public LuaIO(LuaGlobals globals, CIOStream iostream) : base("io", globals) 
    {
        this.iostream = iostream;
        Variant temp_var = iostream.gIOStream.Get("terminal");
        if (temp_var.VariantType == Variant.Type.Object)
        {
            this.terminal = temp_var.AsGodotObject();
        }
    }

    public override Lua Register(Lua state)
    {
        base.Register(state);
        this.state = state;
        // Makes 'print' a global function so you don't have to type 'io.print' every time
        state["print"] = Print;
        return state;
    }

    public override LuaTable PrepTable(Lua state)
    {
        LuaTable tempTable = base.PrepTable(state);
        tempTable["print"] = this.Print;
        tempTable["get_terminal"] = this.GetTerminal;
        return tempTable;
    }

    // Standard print function
    private void Print(Object value)
    {
        // Good for debugging sometimes
        //GD.Print(value);
        this.iostream.push(value.ToString());
    }

    // Terminal functions
    private LuaTable GetTerminal()
    {
        //Lua state = new Lua();
        state.NewTable("terminal");
        LuaTable terminal_table = state.GetTable("terminal");
        if (this.terminal == null)
        {
            return null;
        } else
        {
            terminal_table["get_prefix"] = this.GetTerminalPrefix;
            terminal_table["set_prefix"] = this.SetTerminalPrefix;
            terminal_table["get_output"] = this.GetTerminalOutput;
            terminal_table["set_output"] = this.SetTerminalOutput;
            terminal_table["get_input"] = this.GetTerminalInput;
            terminal_table["set_input"] = this.SetTerminalInput;
        }
        state["terminal"] = null;
        return terminal_table;
    }

    public string GetTerminalPrefix()
    {
        return this.terminal.Call("get_prefix").AsString();
    }

    public void SetTerminalPrefix(string prefix)
    {
        this.terminal.Call("set_terminal_prefix", prefix);
    }

    public string GetTerminalOutput()
    {
        return this.terminal.Call("get_output").AsString();
    }

    public void SetTerminalOutput(string text)
    {
        this.terminal.Call("set_output", text);
    }

    public string GetTerminalInput()
    {
        return this.terminal.Call("get_input").AsString();
    }

    public void SetTerminalInput(string value)
    {
        this.terminal.Call("set_input", value);
    }

    // IOStream stuff
    public LuaTable GetIOStream()
    {
        return iostream.InstanceTable(state);
    }
}
