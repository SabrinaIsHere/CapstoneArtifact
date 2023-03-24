extends ColorRect

signal text_entered(text: String)

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


# Output text to the terminal
func output(text: String) -> void:
	$VBoxContainer/Output.append_text(text + "\n")


func handle_iostream(value: String, size: int) -> void:
	output(value)


func input(text: String) -> void:
	var display: String = $VBoxContainer/InputContainer/InputPrefix.text + " " + text + "\n"
	$VBoxContainer/Output.append_text(display)
	$VBoxContainer/InputContainer/Input.clear()
	emit_signal("text_entered", text)
