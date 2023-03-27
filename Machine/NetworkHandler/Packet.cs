using Godot;
using System;
using NLua;

public partial class Packet : Resource
{
    public string sender;
    public string receiver;
    public int port;
    public string payload;

    public Packet()
    {

    }

    // Sender format 'NetworkID/ObjectID'. NetworkID is -1 if it's on the same network as receiver
    public Packet init(string sender, string receiver, int port, string payload)
    {
        this.sender = sender;
        this.receiver = receiver;
        this.port = port;
        this.payload = payload;
        return this;
    }

    public LuaTable ToTable()
    {
        Lua state = new Lua();
        state.NewTable("packet");
        LuaTable retVal = state.GetTable("table");
        retVal["sender"] = sender;
        retVal["receiver"] = receiver;
        retVal["port"] = port;
        retVal["payload"] = payload;
        return retVal;
    }

    // This is made to interface with the args system in LuaGlobals
    public string ToArg()
    {
        return "args.packet = {" +
            "sender = '" + sender + "'," +
            "receiver = '" + receiver + "'," +
            "port = " + port.ToString() + "," +
            "payload = '" + payload + "'," +
        "}";
    }
}
