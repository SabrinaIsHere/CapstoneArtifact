using Godot;
using Godot.Collections;
using System.Collections.Generic;
using NLua;

public partial class LuaNetwork : LuaLib
{
    GodotObject networkHandler;

    public LuaNetwork(LuaGlobals globals, GodotObject networkHandler) : base("network", globals)
    {
        this.networkHandler = networkHandler;
    }

    public override LuaTable PrepTable(Lua state)
    {
        LuaTable tempTable = base.PrepTable(state);
        tempTable["send_packet"] = SendPacket;
        tempTable["index_network"] = IndexNetwork;
        return tempTable;
    }

    public bool SendPacket(string receiver, int port, string payload)
    {
        Packet packet = new Packet().init(
            networkHandler.Call("format_sender_code").AsString(),
            receiver,
            port,
            payload
        );
        return networkHandler.Call("send_packet", packet).AsBool();
    }

    // If I get around to multiple networks, add a field for network id and have it search peers
    public int[] IndexNetwork()
    {
        GodotObject network = networkHandler.Get("network").AsGodotObject();
        List<int> tags = new List<int>();
        GodotObject[] objects = network.Get("objects").AsGodotObjectArray<GodotObject>();
        foreach (GodotObject i in objects)
        {
            tags.Add(i.Get("id").AsInt32());
        }
        return tags.ToArray();
    }
}
