using Godot;
using System;
using NLua;

public partial class LuaUtil : Resource
{
	public static LuaTable ArrayToTable(Lua state, Object[] list)
	{
		state.NewTable("temp_table");
		LuaTable table = state.GetTable("temp_table");
		state["temp_table"] = null;
		for (int i = 0; i < list.Length; i++)
		{
			table[i + 1] = list[i];
		}
		return table;
	}

	public static LuaTable ArrayToTable(Lua state, int[] list)
	{
		state.NewTable("temp_table");
		LuaTable table = state.GetTable("temp_table");
		state["temp_table"] = null;
		for (int i = 0; i < list.Length; i++)
		{
			table[i + 1] = list[i];
		}
		return table;
	}
}
