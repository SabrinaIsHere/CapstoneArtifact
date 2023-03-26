using Godot;
using System;
using System.Collections.Generic;
using NLua;

// Handles lua interfacing with the games filesystem
public partial class LuaFilesystem : LuaLib
{
    GodotObject filesystem;

    public LuaFilesystem(LuaGlobals globals, GodotObject filesystem) : base("filesystem", globals)
    {

    }
    
    public override LuaTable PrepTable(Lua state)
    {
        LuaTable tempTable = base.PrepTable(state);
        tempTable["get_file"] = GetFile;
        tempTable["get_folder"] = GetFolder;
        return tempTable;
    }

    public LuaTable GetFile(string path)
    {
        return new LuaFile(globals, filesystem, path).ToTable();
    }
    
    public LuaTable GetFolder(string path)
    {
        return new LuaFolder(state, filesystem, path).ToTable();
    }

    public class LuaFile
    {
        LuaGlobals globals;
        Lua state;
        GodotObject filesystem;
        GodotObject file;
        string path;
        bool exists;

        public LuaFile(LuaGlobals globals, GodotObject filesystem, string path)
        {
            this.globals = globals;
            this.state = globals.state;
            this.filesystem = filesystem;
            this.path = path;
            this.file = filesystem.Call("get_object", path).AsGodotObject();
            this.exists = this.file != null && this.file.Get("type").AsString() == "file";
        }

        public void Update()
        {
            file = filesystem.Call("get_object", path).AsGodotObject();
            exists = this.file != null && this.file.Get("type").AsString() == "file";
            if (exists)
            {
                file.Call("update");
            }
        }

        public void Save()
        {
            Update();
            if (exists)
            {
                file.Call("save");
            }
        }

        public LuaTable ToTable()
        {
            if (this.exists)
            {
                state.NewTable("new_file");
                LuaTable retVal = state.GetTable("new_file");
                state["new_file"] = null;
                retVal["type"] = "folder";
                retVal["path"] = path;
                retVal["exists"] = exists;
                retVal["update"] = Update;
                retVal["save"] = Save;
                retVal["get_content"] = GetContent;
                retVal["get_content_as_text"] = GetContentAsText;
                retVal["execute"] = Execute;
                return retVal;
            } else 
            {
                return null;
            }
        }

        public string[] GetContent()
        {
            return file.Get("contents").AsStringArray();
        }

        public string GetContentAsText()
        {
            return file.Call("get_content_as_text").AsString();
        }

        public string Execute()
        {
            return globals.Execute(GetContentAsText());
        }
    }

    public class LuaFolder
    {
        Lua state;
        GodotObject filesystem;
        GodotObject folder;
        string path;
        bool exists;

        public LuaFolder(Lua state, GodotObject filesystem, string path)
        {
            this.state = state;
            this.filesystem = filesystem;
            this.path = path;
            this.folder = filesystem.Call("get_object", path).AsGodotObject();
            this.exists = this.folder != null && this.folder.Get("type").AsString() == "folder";
        }

        public void Update()
        {
            folder = filesystem.Call("get_object", path).AsGodotObject();
            exists = this.folder != null && this.folder.Get("type").AsString() == "folder";
            if (exists)
            {
                folder.Call("update");
            }
        }

        public void Save()
        {
            Update();
            if (exists)
            {
                folder.Call("save");
            }
        }

        public LuaTable ToTable()
        {
            if (this.exists)
            {
                state.NewTable("new_folder");
                LuaTable retVal = state.GetTable("new_folder");
                state["new_folder"] = null;
                retVal["type"] = "folder";
                retVal["path"] = path;
                retVal["exists"] = exists;
                retVal["update"] = Update;
                retVal["save"] = Save;
                retVal["has_file"] = HasFile;
                retVal["has_folder"] = HasFolder;
                retVal["add_file"] = AddFile;
                retVal["add_folder"] = AddFolder;
                retVal["get_file"] = GetFile;
                retVal["get_folder"] = GetFolder;
                return retVal;
            } else 
            {
                return null;
            }
        }

        public bool HasFile(string name)
        {
            return folder.Call("has_object", name, false).AsBool();
        }
        
        public bool HasFolder(string name)
        {
            return folder.Call("has_object", name, true).AsBool();
        }

        public LuaTable AddFile(string name)
        {
            GodotObject obj = folder.Call("add_object", name, false).AsGodotObject();
            if (obj != null && obj.Get("type").AsString() == "file")
            {
                // Will handle this once I've fleshed out more of the file
                return null;
            }
            return null;
        }

        public LuaTable AddFolder(string name)
        {
            GodotObject obj = folder.Call("add_object", name, true).AsGodotObject();
            if (obj != null && obj.Get("type").AsString() == "folder")
            {
                return new LuaFolder(state, filesystem, obj.Call("get_path").AsString()).ToTable();
            }
            return null;
        }

        public LuaTable GetFile(string name)
        {
            GodotObject obj = folder.Call("add_object", name, false).AsGodotObject();
            if (obj != null && obj.Get("type").AsString() == "file")
            {
                // Will handle this once I've fleshed out more of the file
                return null;
            }
            return null;
        }

        public LuaTable GetFolder(string name)
        {
            GodotObject obj = folder.Call("get_object", name, true).AsGodotObject();
            if (obj != null && obj.Get("type").AsString() == "folder")
            {
                return new LuaFolder(state, filesystem, obj.Call("get_path").AsString()).ToTable();
            }
            return null;
        }     
    }
}
