class_name EventHandler extends Resource

const Machine := preload("res://Machine/Machine.gd")
const LuaGlobals := preload("res://Executables/LuaGlobals.cs")


# Machine this is attached to
var machine: Machine

# The categories of events that can be called. Will be loaded from
# the folders under '/evn'
var event_categories: Array[String]

# Lua globals used to execute event scripts
var globals: LuaGlobals


func _init(machine: Machine):
	self.machine = machine
	self.globals = LuaGlobals.new().init(false, ["all"])
	self.event_categories = []
	
	# Init the event categories
	update()


# Update to reflect the event categories of the filesystem
func update() -> void:
	var evn_folder: Folder = machine.filesystem.get_object("/evn")
	if evn_folder == null:
		evn_folder = machine.filesystem.add_folder("/evn")
		if evn_folder == null:
			print("Error: could not find or create 'evn' folder [" + machine.name + "]")
			return
	
	for i in evn_folder.children:
		event_categories.append(i.name)


# Trigger an event. Returns a boolean indicating whether or not the event was found
# Args is an array of lua code snippets executed before the event is triggered
# Use args to effect the globals of the script
func trigger(evn_name: String, evn_category: String, args: Array[String] = []) -> bool:
	evn_name += ".lua"
	var cat_folder = machine.filesystem.get_object("/evn/" + evn_category)
	if cat_folder and cat_folder is Folder:
		var file = cat_folder.get_object(evn_name)
		if file and file is File:
			self.globals.args = args
			file.execute(self.globals)
			return true
	else:
		var evn: FilesystemObject = machine.filesystem.get_object("/evn/" + evn_name)
		if evn and evn is File:
			self.globals.args = args
			evn.execute(self.globals)
			return true
	return false;
