class_name NetworkHandler extends Resource

const Machine := preload("res://Machine/Machine.gd")
const Packet := preload("res://Machine/NetworkHandler/Packet.cs")


var machine: Machine
var network: Network
var id: int


func _init(machine: Machine, network: Network = null):
	self.machine = machine;
	if network:
		self.network = network
	else:
		self.network = Network.new()
		self.network.add_object(self)


# Function that handles a new packet being sent to this machine
func packet_received(packet: Packet) -> void:
	machine.event_handler.trigger("packet_received", "network", [packet.ToArg()])


# Send a packet across this network
func send_packet(packet: Packet) -> bool:
	return network.route_packet(packet)