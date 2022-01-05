public class ConsoleUI : IAppFeedback
{
    public void Message(string txt) => Console.WriteLine(txt);

    public void Error(string     txt) => Console.Error.WriteLine(txt);

    public void Warning(string   txt)=> Console.Error.WriteLine(txt);

    public void Info(string      txt) => Console.WriteLine(txt);

    public void WriteLine(string txt)=> Console.WriteLine(txt);

    public void WriteLine()=> Console.WriteLine();

    public void Debug(string txt)
    {
         Console.Debug.WriteLine(txt);
    }
}
