class_name File extends FilesystemObject

const LuaGlobals := preload("res://Executables/LuaGlobals.cs")

# The globals used to execute this file, if it's ever executed
var globals: LuaGlobals
# Pointer to the actual disk object
var file: FileAccess
# The content of this file. In an array to make lines and general processing easier
var content: PackedStringArray = []
# The type of thing being stored, i.e. 'string' or 'int' or 'lua'
var content_type: String


func _init(filesystem: Filesystem, name: String, parent: Folder):
	super(filesystem, name, parent)
	
	globals = LuaGlobals.new().init(false, ["io"])
	
	# Get the actual object or create it
	var obj_path = filesystem.path + get_path()
	file = FileAccess.open(obj_path, FileAccess.READ_WRITE)
	if not file:
		FileAccess.open(obj_path, FileAccess.WRITE_READ)
	
	file = FileAccess.open(obj_path, FileAccess.READ_WRITE)
	if not file:
		print("Error: Could not find or create file [" + path + "]")
		return
	
	# Get the content of the object
	content = file.get_as_text().split("\n")
	
	
	# Debug, remove later
	if name == "ex.lua":
		print(self.execute())


func update() -> void:
	super()
	content = file.get_as_text().split("\n")


func save() -> void:
	super()
	if not file.file_exists(get_objective_path()):
		FileAccess.open(get_objective_path(), FileAccess.WRITE_READ)
		file = FileAccess.open(get_objective_path(), FileAccess.READ_WRITE)
	file.store_string(get_content_as_text())


# Makes the 'content' array into a plaintext string
func get_content_as_text() -> String:
	var ret_val: String = ""
	for i in content:
		ret_val += i + "\n"
	return ret_val.trim_suffix("\n")


# Executes this file as lua, returns an error message if there is one
func execute(p_globals: LuaGlobals = null) -> String:
	if p_globals:
		return p_globals.Execute(get_content_as_text())
	else:
		return globals.Execute(get_content_as_text())
