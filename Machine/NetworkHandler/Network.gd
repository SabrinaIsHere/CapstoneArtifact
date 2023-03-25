class_name Network extends Resource


const Packet := preload("res://Machine/NetworkHandler/Packet.cs")


var name: String
var id: int
var objects: Array[NetworkHandler] = []
var peers: Array[Network] = []


func _init(name: String = "DefaultNetwork"):
	# Make registry if you implement other networks
	self.id = 0
	self.name = name + "_" + str(self.id)


# Adds an object to this network
func add_object(obj: NetworkHandler) -> bool:
	for i in objects:
		if i == obj:
			return false
	objects.append(obj)
	obj.id = objects.size() - 1
	return true


# Routs a packet to this network or a peer network
# Note, is not yet equipped to handle peer networks
func route_packet(packet: Packet) -> bool:
	var temp_arr: PackedStringArray = packet.receiver.split("/")
	var net_id: int = temp_arr[0].to_int()
	var obj_id: int = temp_arr[1].to_int()
	
	if net_id == -1 or net_id == self.id:
		# Object is in this network
		if obj_id >= objects.size():
			return false
		objects[obj_id].packet_received(packet)
	else:
		# Route to peers to see if this is directed to them
		return false
	
	return true
	
