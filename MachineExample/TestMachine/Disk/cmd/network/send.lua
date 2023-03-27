-- send receiver port payload..

-- send localhost 0 this is a test

if #cmd_args >= 3 then
    local payload = ""
    for i = 3, #cmd_args, 1 do
        payload = payload .. cmd_args[i] .. " "
    end

    if not network.send_packet(cmd_args[1], tonumber(cmd_args[2]), payload) then
        globals.error("could not send packet")
    end
else
    globals.error("usage: send [receiver] [port] [..payload]")
end