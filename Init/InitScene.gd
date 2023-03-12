extends Control

const LuaUtilClass = preload("res://Executables/LuaUtil.cs")
var LuaUtil : LuaUtilClass


# Called when the node enters the scene tree for the first time.
func _ready():
	LuaUtil = LuaUtilClass.new()


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


func _on_terminal_text_entered(text, displayed):
	LuaUtil.ExecuteString(text)
