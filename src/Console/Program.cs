var ui = new ConsoleUI();
var app = new App(ui);
app.DisplayHeader();

if (args.Length == 0)
{
    app.DisplayHelp();
    return 1;
}

var afterwards = InputFileAfterwards.Nothing;
if (HasFlag("rename")) afterwards = InputFileAfterwards.Rename;
if (HasFlag("delete")) afterwards = InputFileAfterwards.Delete;

var cmd = new AppCommand(
    args[0], 
    HasFlag("f"),       
    TimeSpan.FromSeconds(3),
    afterwards,
    !HasFlag("sl")     
    );

return app.Run(cmd);

bool HasFlag(string flag) => args.Any(x => string.Equals(x, "-" + flag, StringComparison.CurrentCultureIgnoreCase));
