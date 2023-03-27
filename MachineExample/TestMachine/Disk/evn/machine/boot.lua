print("Welcome to my Capstone Artifact!")

local shell_color = "fcf802"

-- Make sure the filesystem is configuered correctly
if filesystem.get_folder("/cmd") == nil then
    filesystem.make_folder("/cmd")
end

-- Init globals
globals.color = function (text, color)
    return "[color=#" .. color .. "]" ..text .. "[/color]"
end

globals.error = function (message)
    print("lua: " .. message)
end

globals.current_dir = filesystem.get_folder("/")
globals.update_prefix = function ()
    local terminal = io.get_terminal()
    if not (terminal == nil) then
        local c = "13fc02"
        terminal.set_prefix(globals.color("[", c) .. globals.current_dir.path .. globals.color("]$", "13fc02"))
    end
end
globals.update_prefix()

globals.is_shell = false
globals.set_shell = function(is_shell)
    globals.is_shell = is_shell
    if is_shell then
        local terminal = io.get_terminal()
        print(globals.color(_VERSION .. " Shell", shell_color))
        terminal.set_prefix(globals.color(">>>", shell_color))
    else
        globals.update_prefix()
    end
end

globals.network = {}
globals.network.blacklist = {}