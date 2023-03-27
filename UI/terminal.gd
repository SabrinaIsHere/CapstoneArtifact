extends ColorRect

signal text_entered(text: String)


# Handles pressing the key up or down in the terminal to go through
# previously entered commands
var entries_index = 1
var entries: Array[String] = []


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
	if val.is_empty():
		$VBoxContainer/Output.clear()
		return
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
	entries.append(text)
	entries_index = entries.size()
	emit_signal("text_entered", text)


# This function was not fun to write
func _on_input_gui_input(event):
	if event is InputEventKey:
		if event.pressed:
			if event.keycode == KEY_UP:
				if entries_index - 1 >= 0 and entries.size() > 0:
					entries_index -= 1
					set_input(entries[entries_index])
			elif event.keycode == KEY_DOWN:
				if entries_index + 1 < entries.size():
					entries_index += 1
					set_input(entries[entries_index])
				else:
					entries_index = entries.size()
					set_input("")
