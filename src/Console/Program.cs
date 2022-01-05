var ui = new ConsoleUI();
var app = new App(ui);
app.DisplayHeader();


if (args.Length == 0)
{
    app.DisplayHelp();
    return 1;
}

var cmd = new AppCommand(
    args[0], 
    HasFlag("f"),       // _F_orce Overwrite 
    TimeSpan.FromSeconds(3),
    InputFileAfterwards.Rename,
    HasFlag("sl")       // _S_kip _L_aunch
    );

return app.Run(cmd);

bool HasFlag(string flag) => args.Any(x => string.Equals(x, "-" + flag, StringComparison.CurrentCultureIgnoreCase));
