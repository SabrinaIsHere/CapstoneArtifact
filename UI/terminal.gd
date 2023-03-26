extends ColorRect

signal text_entered(text: String)

# Get the text displayed before the area where the user is able to type
func get_prefix() -> String:
	return $VBoxContainer/InputContainer/InputPrefix.text


# Set the text in front of the input box
func set_prefix(val: String) -> void:
	$VBoxContainer/InputContainer/InputPrefix.text = val


# Get the text being displayed on screen
func get_output() -> String:
	return $VBoxContainer/Output.text


# Set the text being displayed on screen
func set_output(val: String) -> void:
	$VBoxContainer/Output.text = val


# Get the text in the input box
func get_input() -> String:
	return $VBoxContainer/InputContainer/Input.text


# Set the input in the input box
func set_input(value: String) -> void:
	$VBoxContainer/InputContainer/Input.text = value


# Output text to the terminal
func output(text: String) -> void:
	$VBoxContainer/Output.append_text(text + "\n")


func handle_iostream(value: String, size: int) -> void:
	output(value)


func input(text: String) -> void:
	# Going to be handled in lua
#	var display: String = $VBoxContainer/InputContainer/InputPrefix.text + " " + text + "\n"
#	$VBoxContainer/Output.append_text(display)
#	$VBoxContainer/InputContainer/Input.clear()
	emit_signal("text_entered", text)
