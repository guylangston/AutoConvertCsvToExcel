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
Console.WriteLine($"  [Input] {info.FullName}");

var outFilePath = Path.Combine(info.DirectoryName, Path.GetFileNameWithoutExtension(info.Name) + ".xlsx");
Console.WriteLine($" [Output] {outFilePath}");

if (File.Exists(outFilePath))
{
    if (args.Any(x=>x.Equals("-f", StringComparison.CurrentCultureIgnoreCase)))
    {
        Console.WriteLine($"WARNING: Overwiting {outFilePath}");
    }
    else
    {
        Console.Error.WriteLine("Output file already exists... Stopping");
        return -4;
    }
    
}

// Create new workbook
var wb = new NPOI.XSSF.UserModel.XSSFWorkbook();
var sheet = wb.CreateSheet();

var dataFormatCustom = wb.CreateDataFormat();
var styleDate = wb.CreateCellStyle();
styleDate.DataFormat = dataFormatCustom.GetFormat("yyyy-MM-dd");

var styleDateTime = wb.CreateCellStyle();
styleDateTime.DataFormat = dataFormatCustom.GetFormat("yyyy-MM-dd hh:mm:ss");


// Read Input
var readerTxt = File.OpenText(inp);
var rowIdx = 0;
var cellCount = 0;
var reader = new SerialReaderCSV(readerTxt);
foreach (var row in reader.ForEachRow())
{
    var colIdx = 0;

    var outRow = sheet.CreateRow(rowIdx);
    foreach (var cell in row)
    {
        var outCell = outRow.CreateCell(colIdx);

        if (DateTime.TryParse(cell, out var dt))
        {
            outCell.SetCellValue(dt);
            outCell.CellStyle = (dt.TimeOfDay.TotalSeconds == 0)
             ? styleDate
             : styleDateTime;

        }
        else if (double.TryParse(cell, out var num))
        {
            outCell.SetCellValue(num);
        }
        else
        {
            outCell.SetCellValue(cell);
        }
        

        cellCount++;
        colIdx++;
    }

    rowIdx++;
}


using var outFile = File.Create(outFilePath);
wb.Write(outFile);

Console.WriteLine($"[Convert] Rows: {rowIdx}; Cells: {cellCount}");

Console.WriteLine($" [Launch] Excel (or associated program from .XLSX)");
System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
{
    UseShellExecute = true,
    FileName = outFilePath
});
return 0;