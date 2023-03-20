-- Libraries and bindings go here
local env = {}

local function run(untrusted_code)
    if untrusted_code:byte(1) == 27 then return nil, "binary bytecode prohibited" end
    local untrusted_function, message = load(untrusted_code)
    if not untrusted_function then return nil, message end
    untrusted_function._ENV = env
    return pcall(untrusted_function)
end