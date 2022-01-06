public interface IAppFeedback
{
    void Message(string txt);
    void Error(string   txt);
    void Warning(string txt);
    void Info(string    txt);

    void WriteLine(string txt);
    void WriteLine();

    void Debug(string txt);
}

public enum InputFileAfterwards
{
    Nothing,
    Delete,
    Rename
}


public record AppCommand(string InputFile, bool Overwrite, TimeSpan DelayBeforeClose, InputFileAfterwards Afterwards, bool Launch)
{
    public string? OutFile { get; set; }
}


public class App
{
    private IAppFeedback ui;

    public App(IAppFeedback ui)
    {
        this.ui = ui;
    }

    public int Run(AppCommand cmd)
    {
        if (cmd == null || string.IsNullOrWhiteSpace(cmd.InputFile))
        {
            ui.Error("No files given. Exiting");
            DisplayHelp();
            return 1;
        }

        if (!string.Equals(Path.GetExtension(cmd.InputFile), ".csv", StringComparison.InvariantCultureIgnoreCase))
        {
            ui.Error("Input file is not .CSV");
            DisplayHelp();
            return 2;
        }

        if (!File.Exists(cmd.InputFile))
        {
            ui.Error("Input File does not exit. Exiting...");
            DisplayHelp();
            return 3;
        }

        var info = new FileInfo(cmd.InputFile);
        ui.WriteLine($"  [Input] {info.FullName}");

        if (cmd.OutFile == null)
        {
            cmd.OutFile = Path.Combine(
                  info.DirectoryName ?? throw new Exception("Dir required"), 
                  Path.GetFileNameWithoutExtension(info.Name) + ".xlsx");
        }

        ui.WriteLine($" [Output] {cmd.OutFile}");
        if (File.Exists(cmd.OutFile))
        {
            if (cmd.Overwrite)
            {
                ui.Warning($"WARNING: Overwiting {cmd.OutFile}");
            }
            else
            {
                ui.Error("Output file already exists... Stopping");
                DisplayHelp();
                return 4;
            }
        }

        // Create new workbook
        var wb    = new NPOI.XSSF.UserModel.XSSFWorkbook();
        var sheet = wb.CreateSheet();

        var dataFormatCustom = wb.CreateDataFormat();
        var styleDate        = wb.CreateCellStyle();
        styleDate.DataFormat = dataFormatCustom.GetFormat("yyyy-MM-dd");

        var styleDateTime = wb.CreateCellStyle();
        styleDateTime.DataFormat = dataFormatCustom.GetFormat("yyyy-MM-dd hh:mm:ss");

        // Read Input
        using (var readerTxt = File.OpenText(cmd.InputFile))
        {
            var rowIdx    = 0;
            var cellCount = 0;
            var reader    = new SerialReaderCSV(readerTxt);
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
            
            ui.WriteLine($"[Convert] Rows: {rowIdx}; Cells: {cellCount}");
        }

        switch (cmd.Afterwards)
        {
            case InputFileAfterwards.Delete:
                File.Delete(cmd.InputFile);
                ui.Info($"[Deleted] Input file {cmd.InputFile}");
                break;
            
            case InputFileAfterwards.Nothing:
                break;
            
            case InputFileAfterwards.Rename:
                var renTo = cmd.InputFile + "-converted.bak-csv";
                File.Move(cmd.InputFile, renTo, cmd.Overwrite);
                ui.Info($"[Renamed] Input file {Path.GetFileName(cmd.InputFile)}");
                ui.Info($"[Renamed]         As {Path.GetFileName(renTo)}");
                break;

            default:
                throw new NotImplementedException();
        }

        using var outFile = File.Create(cmd.OutFile);
        wb.Write(outFile);

        if (cmd.Launch)
        {
            ui.WriteLine($" [Launch] Excel (or associated program from .XLSX)");
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                UseShellExecute = true,
                FileName        = cmd.OutFile
            });    
        }
        
        return 0;
    }

    public void DisplayHeader()
    {
        ui.WriteLine("AutoConvertCsvToExcel - Convert .CSV to .XSLX then open. Version 1.1");
        ui.WriteLine("https://github.com/guylangston/AutoConvertCsvToExcel");
        ui.WriteLine();
    }


    public void DisplayHelp()
    {
        ui.WriteLine("usage: AutoConvertCsvToExcel.exe {input-file.csv}");
        ui.WriteLine(" -f         Overwrite target");
        ui.WriteLine(" -sl        Skip Launch (associated OS program)");
        ui.WriteLine(" -delete    After import, DELETE source file");
        ui.WriteLine(" -rename    After import, RENAME source file");
    }
}
