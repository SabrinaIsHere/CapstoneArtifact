extends ColorRect

signal text_entered(text: String, displayed: String)

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass


# Output text to the terminal
func output(text: String) -> void:
	var display: String = "\n" + $VBoxContainer/InputContainer/InputPrefix.text + " " + text
	$VBoxContainer/Output.append_text(display)
	emit_signal("text_entered", text, display)


func input(text: String) -> void:
	output(text)
	$VBoxContainer/InputContainer/Input.clear()
