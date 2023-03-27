extends Control


const Machine := preload("res://Machine/Machine.gd")

var network: Network


# Called when the node enters the scene tree for the first time.
func _ready():
	network = Network.new()
	for i in get_children():
		if i is Machine:
			network.add_object(i.network_handler)


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
