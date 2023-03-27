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
    return
end

if not (target == nil) then
    globals.error(path .. ": target already exists")
else
    local val = filesystem.make_file(path)
    if val == nil then
        globals.error(path .. ": could not create file")
    end
end