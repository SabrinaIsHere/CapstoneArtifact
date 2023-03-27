local terminal = io.get_terminal()
if not (terminal == nil) then
    terminal.set_output("")
else
    globals.error("no terminal found")
end