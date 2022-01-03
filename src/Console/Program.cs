if (args.Length == 0)
{
    Console.Error.WriteLine("No files given. Exiting");
    return -1;
}

var inp = args[0];
if (!string.Equals(Path.GetExtension(inp), ".csv", StringComparison.InvariantCultureIgnoreCase))
{
    Console.Error.WriteLine("Input file is not .CSV");
    return -2;
}

if (!File.Exists(inp))
{
    Console.Error.WriteLine("Input File does not exit. Exiting...");
    return -3;
}

var info = new FileInfo(inp);
Console.WriteLine($"Input File: {info.FullName}");

// Create new workbook

// Read Input
var readerTxt = File.OpenText(inp);
var rowIdx = 0;
var cellCount = 0;
var reader = new SerialReaderCSV(readerTxt);
foreach (var row in reader.ForEachRow())
{
    var colIdx = 0;
    foreach (var cell in row)
    {
        Console.WriteLine(cell);
        cellCount++;
        colIdx++;
    }

    rowIdx++;
}

Console.WriteLine($"Rows Read: {rowIdx}; Total Cells: {cellCount}");
return 0;