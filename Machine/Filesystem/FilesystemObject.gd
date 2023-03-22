class_name FilesystemObject extends Object

# The filesystem managing this file
var filesystem: Filesystem
# Filename
var name: String
# Path the root of this object's filesystem
var master_path: String
# Relative path to this object from the filesystem pov. Will always end in '/'
var path: String
# The folder containing this object
var parent: Folder


func _init(filesystem: Filesystem, name: String = "", parent: Folder = null):
	self.name = name
	self.parent = parent
	self.filesystem = filesystem

	# Get path of this object
	if parent:
		self.path = parent.path + name + "/"
	else:
		self.path = "/"
	
	# Assumes that the checks for this have already been made, either in
	# add_object or otherwise
	if parent:
		parent.children.append(self)


func _to_string():
	return "Filesystem Object [" + get_path() + "]"


# Gets the path to this object without a slash at the end of the path
# if this is used with root it will return a blank string
func get_path() -> String:
	if self.path == "/":
		return ""
	else:
		return self.path.substr(0, self.path.length() - 1)


# Returns an object disk path to this object
func get_objective_path() -> String:
	return filesystem.path + get_path()


# Deletes this object on the disk and from the parents 'children' array
func delete() -> void:
	if parent:
		parent.children.erase(self)
		DirAccess.open(parent.get_path()).remove(name)
		self.free()
	else:
		print("Error: Attempted to erase assumed root object [" + name + "]")


# Updates object to match the disk partner
func update() -> void:
	# Deletes this if it doesn't exist on the disk
	# Updating data more specific to files or folders is done in the respective classes
	if parent:
		var temp_dir = DirAccess.open(parent.get_objective_path())
		if !(temp_dir.dir_exists(name) or temp_dir.file_exists(name)):
			delete()


# Updates disk to match this object
func save() -> void:
	# This is mostly meant to be handled in overrides
	pass
