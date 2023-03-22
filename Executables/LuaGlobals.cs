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
    public static LuaLib[] LibRegistry = new LuaLib[] {
        new LuaIO()
    };

    public bool isPersistant;
    public Lua state;
    public string[] perms;
    public List<LuaLib> libs;
    // List of strings executed before any lua code is run, to affect globals
    public string[] args;

    public LuaGlobals()
    {
        
    }

    // You have to do intialization here instead of the constructor to handle
    // a weird quirk I'm having with instancing from GDScript
    public LuaGlobals init(bool isPersistant, string[] perms)
    {
        this.isPersistant = isPersistant;
        this.perms = perms;
        ProcessPerms();
        this.state = PrepState();
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
        Lua tempState = new Lua();
        tempState.DoString("_ENV = {}");
        foreach (LuaLib i in this.libs)
        {
            i.Register(tempState);
        }
        return tempState;
    }

    // Execute a string of lua code and return any error messages, if there are none null is returned
    public string Execute(string text)
    {
        // Handle args
        if (this.args != null && this.args.Length > 0)
        {
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
        }

        if (!isPersistant)
        {
            this.state = PrepState();
        }
        
        return errorMsg;
    }
}
