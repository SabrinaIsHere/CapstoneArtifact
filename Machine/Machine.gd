extends Control

const LuaUtilClass = preload("res://Executables/LuaUtil.cs")
var LuaUtil: LuaUtilClass

# Name of this machine
@export var machine_name: String

# Path to this machines data
var path: String
# Actual folder object
var dir: DirAccess

# Machine components
var filesystem: Filesystem


# Called when the node enters the scene tree for the first time.
func _ready():
	# Prepping lua execution
	LuaUtil = LuaUtilClass.new()
	
	# Establishing machine data folder
	path = "user://" + machine_name
	dir = DirAccess.open(path)
	if not dir:
		DirAccess.open("user://").make_dir(machine_name)
		dir = DirAccess.open(path)
		if not dir:
			print("Error: Could not find or create machine [" + machine_name + "] folder")
			return
	
	#OS.shell_open(ProjectSettings.globalize_path(path))
	filesystem = Filesystem.new(self)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


# This is kind of defunct, I'm going to be replacing this with IOStreams at some point
func _process_terminal_input(input: String) -> void:
	pass
