local file_color = "13fc02"
local folder_color = "1154fc"

local target
local path

if #cmd_args == 1 then
    if cmd_args[1]:sub(1, 1) == "/" then
        path = cmd_args[1]
    else
        path = globals.current_dir.path
        if path:sub(#path) == "/" then
            path = path .. cmd_args[1]
        else
            path = path .. "/" .. cmd_args[1]
        end
    end
    target = filesystem.get_folder(path)
else
    target = globals.current_dir
end


if not (target == nil) and target.type == "folder" then
    local output = ""
    local children = target.get_children()
    for i, v in ipairs(children) do
        if v.type == "file" then
            output = output .. globals.color(v.name, file_color) .. "      "
        else
            output = output .. globals.color(v.name, folder_color) .. "      "
        end
    end
    if not (output == "") then
        print(output)
    end
else
    globals.error(path .. ": no such directory")
end