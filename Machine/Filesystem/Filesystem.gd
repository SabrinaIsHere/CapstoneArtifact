class_name Filesystem extends Resource

# Misc class types
const Machine := preload("res://Machine/Machine.gd")

# Actual root disk file
var path: String
var dir: DirAccess

# Machine this is handling files for
var machine: Machine

# Root folder of the system
var root: Folder

func _init(machine: Machine):
	self.machine = machine
	
	path = machine.path + "/Disk"
	
	# Verify disk folder
	if not machine.dir.dir_exists("Disk"):
		machine.dir.make_dir("Disk")
	
	dir = DirAccess.open(machine.path + "/Disk")
	if not dir:
		print("Could not open or create disk file for [" + machine.machine_name + "]")
		return
	
	# Initialize this from disk or create new default filesystem
	root = Folder.new(self, "root", null)
	root.add_object("evn", true)


# Will check to ensure that the filesystem is up to date with the disk
func update() -> void:
	root.update()


# Gets an object from a path. Returns null if it can't be found
func get_object(path: String) -> FilesystemObject:
	if path.strip_edges() == "/":
		return root
	var parts = path.split("/", false)
	var parent: Folder = root
	var obj: FilesystemObject = null
	for i in parts:
		var temp_obj
		if parent is Folder:
			temp_obj = parent.get_object(i)
		else:
			return null
		
		if temp_obj == null:
			return null
		
		parent = temp_obj
		obj = temp_obj
	return obj


# Add a file at the given path. Return null if it could not
func add_file(path: String) -> FilesystemObject:
	var parent_path = path.substr(0, path.rfind("/"))
	var file_name = path.substr(path.rfind("/") + 1, -1)
	var parent = get_object(parent_path)
	if parent and parent is Folder:
		return parent.add_object(file_name, false)
	else: return null


# Adds a folder at the given path. Returns null if it could not
func add_folder(path: String) -> FilesystemObject:
	var parent_path = path.substr(0, path.rfind("/") + 1)
	var folder_name = path.substr(path.rfind("/") + 1, -1)
	var parent := get_object(parent_path)
	if parent and parent is Folder:
		return parent.add_object(folder_name, true)
	else: return null
