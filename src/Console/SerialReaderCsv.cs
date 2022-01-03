public class SerialReaderCSV
{
    private readonly TextReader reader;

    public SerialReaderCSV(TextReader reader)
    {
        this.reader = reader;
        Row = Column = 0;
        IsEndOfFile = false;
    }

    public int Row { get; private set; }
    public int Column { get; private set; }
    public bool IsEndOfFile { get; private set; }

    public IEnumerable<List<string>> ForEachRow()
    {
        string? line = null;
        while ((line = reader.ReadLine()) != null)
        {
            yield return ParseSingleLine(line);
        }

        IsEndOfFile = true;
    }

    public static List<string> ParseSingleLine(string line)
    {
        // http://stackoverflow.com/questions/17207269/how-to-properly-split-a-csv-using-c-sharp-split-function
        int i;
        int a = 0;
        int count = 0;
        List<string> result = new List<string>();
        for (i = 0; i < line.Length; i++)
        {
            switch (line[i])
            {
                case ',':
                    if ((count & 1) == 0)
                    {
                        result.Add(line.Substring(a, i - a).Trim('"'));
                        a = i + 1;
                    }

                    break;

                //case '\'':
                case '"':

                    count++;
                    break;
            }
        }

        result.Add(line.Substring(a).Trim('"'));

        return result;
    }
}