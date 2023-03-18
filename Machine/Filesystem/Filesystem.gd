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
