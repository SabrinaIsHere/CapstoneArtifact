using Godot;
using System;
using NLua;
using System.Collections.Generic;

// This class stores config data about globals used to execute scripts
// This class will handle persistance, library perms, etc.

// Todo: make this handle iostreams
public partial class LuaGlobals : Resource
{
    // All usable libraries. This will be added according to the 'perms' array
    public LuaLib[] LibRegistry;

    public bool isPersistant;
    public Lua state;
    public string[] perms;
    public List<LuaLib> libs;
    // List of strings executed before any lua code is run, to affect globals
    public string[] args;
    public CIOStream iostream;
    public LuaTable scriptGlobals;

    public LuaGlobals()
    {
        
    }

    // You have to do intialization here instead of the constructor to handle
    // a weird quirk I'm having with instancing from GDScript
    public LuaGlobals init(bool isPersistant, string[] perms, GodotObject iostream, GodotObject machine)
    {
        // for the benefit of library initialization
        this.state = new Lua();
        this.isPersistant = isPersistant;
        this.perms = perms;
        this.iostream = (CIOStream) iostream.Get("interface");
        this.LibRegistry = new LuaLib[] {
            new LuaIO(this, this.iostream),
            new LuaFilesystem(this, machine.Get("filesystem").AsGodotObject()),
            new LuaEvent(this, machine.Get("event_handler").AsGodotObject()),
            new LuaNetwork(this, machine.Get("network_handler").AsGodotObject()),
            new LuaMachine(this, machine)
        };
        ProcessPerms();
        this.state = PrepState();
        this.state.NewTable("globals");
        this.scriptGlobals = state.GetTable("globals");
        return this;
    }

    // Gather and store instances of the allowed libraries
    // This is done to speed up the resetting of states
    public void ProcessPerms()
    {
        libs = new List<LuaLib>();

        // If any of the perms are 'all', load all available libraries
        foreach (string i in this.perms)
        {
            if (i.Equals("all"))
            {
                foreach (LuaLib j in LibRegistry)
                {
                    libs.Add(j);
                }
                return;
            }
        }

        // Load libraries as requested in 'perms'
        foreach (string i in this.perms)
        {
            foreach (LuaLib j in LibRegistry)
            {
                if (i.Equals(j.name))
                {
                    libs.Add(j);
                    break;
                }
            }
        }
    }

    // Sets up a new, clean state
    public Lua PrepState()
    {
        state.DoString("_ENV = {}");
        foreach (LuaLib i in this.libs)
        {
            i.Register(state);
        }
        state["globals"] = scriptGlobals;
        return state;
    }

    // Execute a string of lua code and return any error messages, if there are none null is returned
    public string Execute(string text, bool keep_state)
    {
        // Handle args
        if (this.args != null && this.args.Length > 0)
        {
            // Establish default location for passed args. Can be ignored
            state.DoString("args = {}");
            foreach (string i in this.args)
            {
                state.DoString(i);
            }
        }

        string errorMsg = null;
        try {
            state.DoString(text);
        } catch (Exception e) {
            errorMsg = e.ToString();
            if (!iostream.gIOStream.Get("terminal").Equals(null))
            {
                errorMsg = "[color=#fc0202]" + errorMsg + "[/color]";
            }
            iostream.push(errorMsg);
        }

        if (!isPersistant || !keep_state)
        {
            this.scriptGlobals = this.state.GetTable("globals");
            this.state = PrepState();
        }
        
        return errorMsg;
    }

    public string Execute(string text)
    {
        return Execute(text, false);
    }
}
