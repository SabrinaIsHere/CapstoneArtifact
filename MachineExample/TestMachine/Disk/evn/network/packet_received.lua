--[[
    (yes I'm aware this isn't how ports work irl but it's easier than parsing payloads) and I can fix it later
    protocol -
        0: ping initiate
        1: ping response
        2: tcp initiate (send data)
        3: shell initiate (can I execute commands?)
        4: shell response (yes/no)
        5: shell resolution (executing payload as a command)
        6: shell output (sending shell output back)
]]

local packet = args.packet
local port = packet.port

for index, value in ipairs(globals.network.blacklist) do
    if value == packet.sender then
        return
    end
end

function nprint(msg)
    print("[" .. packet.sender .. ", " .. tostring(packet.port) .. "]: " .. msg)
end

if port == 0 then
    network.send_packet(packet.sender, 1, "")
elseif port == 1 then
    nprint("Pong!")
elseif port == 2 then
    nprint(packet.payload)
end