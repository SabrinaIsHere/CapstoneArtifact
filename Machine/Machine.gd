extends Control

const LuaUtilClass = preload("res://Executables/LuaUtil.cs")
var LuaUtil: LuaUtilClass

# Name of this machine
@export var machine_name: String

# Path to this machines data
var path: String
# Actual folder object
var dir: DirAccess

# Default terminal IOStream
var terminal_iostream: IOStream

# Machine components
var filesystem: Filesystem
var event_handler: EventHandler


# Called when the node enters the scene tree for the first time.
func _ready():
	# Prepping lua execution
	LuaUtil = LuaUtilClass.new()
	
	terminal_iostream = IOStream.new($Terminal)
	
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
	event_handler = EventHandler.new(self)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


# What happens when the user gives input
# Consider moving this to IOStreams
func _process_terminal_input(input: String) -> void:
	event_handler.trigger("terminal_input", "input", ["
		input = \"%s\"" % input])


# Updates components in the machine, called once per second
func update():
	filesystem.update()
	event_handler.update()
