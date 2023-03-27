terminal = io.get_terminal()

terminal.set_input("")
print(terminal.get_prefix() .. " " .. args.input)

if globals.is_shell then
    if args.input == "exit" then
        globals.set_shell(false)
        return
    end

    local ret = machine.execute(args.input)
    if not (ret == nil) then
        print(ret)
    end
    return
end

local function search_children(folder, cmd)
    local children = folder.get_children()
    for index, child in ipairs(children) do
        if child.type == "file" and child.name == (cmd .. ".lua") then
            return child
        elseif child.type == "folder" then
            local result = search_children(child, cmd)
            if not (result == nil) then
                return result
            end
        end
    end
end

cmd = ""
cmd_args = {}

local pos = 1
for str in string.gmatch(args.input, "[^%s]+") do
    cmd_args[pos] = str
    pos = pos + 1
end

cmd = cmd_args[1]
cmd_args = table.move(cmd_args, 2, #cmd_args, 1)
cmd_args[#cmd_args] = nil

local cmd_folder = filesystem.get_folder("/cmd")

local cmd_file = search_children(cmd_folder, cmd)

if cmd_file == nil then
    globals.error(cmd .. ": command not found")
    return
else
    local ret_val = cmd_file.execute()
    if not (ret_val == nil) then
        print(ret_val)
    end
end