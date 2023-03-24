# This class mostly exists for the benefit of lua stuff later so that it can
# more extensibly influence what happens to output. Basically, lua scripts'
# output goes through an IOStream which lua scripts can control and do stuff with
class_name IOStream extends Object

# waiting includes the output just added
signal new_output(output: String, waiting: int)

const CIOStream = preload("res://Machine/IOStream/CIOStream.cs")
const Terminal = preload("res://UI/terminal.gd")


# The held output queue
var output: Array[String] = []

# The interface with the lua side of things
var interface: CIOStream


func _init(terminal: Terminal = null, interface: CIOStream = null):
	if terminal:
		new_output.connect(Callable(terminal, "handle_iostream"))
	if interface:
		self.interface = interface
	else:
		self.interface = CIOStream.new()
		self.interface.init(self)


# Adds new value to the end of the stream
func push(new_val: String) -> void:
	var ret = interface.PushEvent(new_val)
	if ret and ret is String:
		new_val = ret
	output.append(new_val)
	emit_signal("new_output", new_val, output.size())


# Gets the earliest added item, removes it, and returns it
func pop() -> String:
	var ret_val := output[0]
	var ex
	ex = interface.PopEvent(ret_val)
	if ex and ex is String:
		return ex
	output.remove_at(0)
	return ret_val
