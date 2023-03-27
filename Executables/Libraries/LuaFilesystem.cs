using Godot;
using Godot.Collections;
using System;
using NLua;

// Handles lua interfacing with the games filesystem
public partial class LuaFilesystem : LuaLib
{
    GodotObject filesystem;

    public LuaFilesystem(LuaGlobals globals, GodotObject filesystem) : base("filesystem", globals)
    {
        this.filesystem = filesystem;
    }
    
    public override LuaTable PrepTable(Lua state)
    {
        LuaTable tempTable = base.PrepTable(state);
        tempTable["get_file"] = GetFile;
        tempTable["get_folder"] = GetFolder;
        tempTable["make_file"] = MakeFile;
        tempTable["make_folder"] = MakeFolder;
        return tempTable;
    }

    public LuaTable GetFile(string path)
    {
        return new LuaFile(globals, filesystem, path).ToTable();
    }
    
    public LuaTable GetFolder(string path)
    {
        return new LuaFolder(globals, filesystem, path).ToTable();
    }

    public LuaTable MakeFile(string path)
    {
        filesystem.Call("add_file", path);
        return new LuaFile(globals, filesystem, path).ToTable();
    }
    
    public LuaTable MakeFolder(string path)
    {
        filesystem.Call("add_folder", path);
        return new LuaFolder(globals, filesystem, path).ToTable();
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
                retVal["type"] = "file";
                retVal["name"] = file.Get("name").AsString();
                retVal["path"] = path;
                retVal["exists"] = exists;
                retVal["update"] = Update;
                retVal["save"] = Save;
                retVal["get_content"] = GetContent;
                retVal["get_content_as_text"] = GetContentAsText;
                retVal["execute"] = Execute;
                retVal["delete"] = Delete;
                return retVal;
            } else 
            {
                return null;
            }
        }

        public LuaTable GetContent()
        {
            return LuaUtil.ArrayToTable(state, file.Get("contents").AsStringArray());
        }

        public string GetContentAsText()
        {
            return file.Call("get_content_as_text").AsString();
        }

        public string Execute()
        {
            return globals.Execute(GetContentAsText(), true);
        }

        public void Delete()
        {
            file.Call("delete");
            exists = false;
        }
    }

    public class LuaFolder
    {
        LuaGlobals globals;
        Lua state;
        GodotObject filesystem;
        GodotObject folder;
        string path;
        bool exists;

        public LuaFolder(LuaGlobals globals, GodotObject filesystem, string path)
        {
            this.globals = globals;
            this.state = globals.state;
            this.filesystem = filesystem;
            this.path = path;
            Variant temp = filesystem.Call("get_object", path);
            if (temp.Equals(null))
            {
                this.exists = false;
            } else 
            {
                this.folder = temp.AsGodotObject();
                this.exists = this.folder != null && this.folder.Get("type").AsString() == "folder";
            }
            if (exists)
            {
                this.path = folder.Call("get_path").AsString();
                if (this.path == "")
                {
                    this.path = "/";
                }
            }
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
                retVal["name"] = folder.Get("name").AsString();
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
                retVal["get_children"] = GetChildren;
                retVal["delete"] = Delete;
                retVal["open"] = Open;
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
                return new LuaFile(globals, filesystem, obj.Call("get_path").AsString()).ToTable();
            }
            return null;
        }

        public LuaTable AddFolder(string name)
        {
            GodotObject obj = folder.Call("add_object", name, true).AsGodotObject();
            if (obj != null && obj.Get("type").AsString() == "folder")
            {
                return new LuaFolder(globals, filesystem, obj.Call("get_path").AsString()).ToTable();
            }
            return null;
        }

        public LuaTable GetFile(string name)
        {
            GodotObject obj = folder.Call("add_object", name, false).AsGodotObject();
            if (obj != null && obj.Get("type").AsString() == "file")
            {
                return new LuaFile(globals, filesystem, obj.Call("get_path").AsString()).ToTable();
            }
            return null;
        }

        public LuaTable GetFolder(string name)
        {
            GodotObject obj = folder.Call("get_object", name, true).AsGodotObject();
            if (obj != null && obj.Get("type").AsString() == "folder")
            {
                return new LuaFolder(globals, filesystem, obj.Call("get_path").AsString()).ToTable();
            }
            return null;
        }

        public LuaTable GetChildren()
        {
            Array<GodotObject> children = folder.Get("children").AsGodotArray<GodotObject>();
            state.NewTable("temp_table");
            LuaTable retVal = state.GetTable("temp_table");
            state["temp_table"] = null;
            for (int i = 0; i < children.Count; i++)
            {
                GodotObject child = children[i];
                if (child.Get("type").AsString() == "file")
                {
                    retVal[i + 1] = new LuaFile(globals, filesystem, child.Call("get_path").AsString()).ToTable();
                } else if (child.Get("type").AsString() == "folder")
                {
                    retVal[i + 1] = new LuaFolder(globals, filesystem, child.Call("get_path").AsString()).ToTable();
                }
            }
            return retVal;
        }

        public void Delete()
        {
            folder.Call("delete");
            exists = false;
        }

        public void Open()
        {
            folder.Call("open");
        }
    }
}
