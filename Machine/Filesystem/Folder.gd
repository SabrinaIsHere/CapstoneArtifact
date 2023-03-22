class_name Folder extends FilesystemObject

# Actual folder associated with this
var dir: DirAccess
# Children of this folder
var children: Array[FilesystemObject] = []
# Whether or not this is the root of the filesystem
var is_root: bool


func _init(filesystem: Filesystem, name: String, parent: Folder):
	super(filesystem, name, parent)
	if not parent:
		is_root = true

	# Get the actual object or create it
	var obj_path = filesystem.path + get_path()
	dir = DirAccess.open(obj_path)
	if not dir:
		DirAccess.open(filesystem.path + parent.get_path()).make_dir(name)
	
	dir = DirAccess.open(obj_path)
	if not dir:
		print("Error: Could not find or create folder [" + path + "]")
		return
	
	# Initialize children from disk objects
	var child_dirs: PackedStringArray = dir.get_directories()
	var child_files: PackedStringArray = dir.get_files()
	
	for i in child_dirs:
		add_object(i, true)
	
	for i in child_files:
		add_object(i, false)


func update():
	super()
	var child_dirs: PackedStringArray = dir.get_directories()
	var child_files: PackedStringArray = dir.get_files()
	
	for i in child_dirs:
		add_object(i, true)
	
	for i in child_files:
		add_object(i, false)
	
	for i in children:
		if !(child_dirs.has(i.name) or child_files.has(i.name)):
			i.delete()


func save():
	super()
	for i in children:
		i.save()


# Checks for whether or not the folder contains an object of the name
# and type designated
func has_object(obj_name: String, is_folder: bool) -> bool:
	for i in children:
		if (i.name == obj_name) and (is_folder == i is Folder):
			return true
	return false


# Adds an object of the name and type designated. 
# Returns true if successful, false if not.
func add_object(obj_name: String, is_folder: bool) -> FilesystemObject:
	if not has_object(obj_name, is_folder):
		if is_folder:
			return Folder.new(filesystem, obj_name, self)
		else:
			return File.new(filesystem, obj_name, self)
	else:
		return null


# Gets a child object from it's name. Returns null if it doesn't exist
func get_object(obj_name: String) -> FilesystemObject:
	for i in children:
		if i.name == obj_name:
			return i
	return null
