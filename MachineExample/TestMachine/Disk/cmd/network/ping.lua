if #cmd_args == 1 then
    if not network.send_packet(cmd_args[1], 0, "") then
        globals.error("could not send packet")
    end
else
    globals.error("usage: send [receiver]")
end