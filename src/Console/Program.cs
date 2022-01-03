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

Console.WriteLine($"Input File: {inp}");

// Create new workbook

// Read Input
var reader = File.OpenText(inp);
string? ln = null;
var lnCount = 0;
while( (ln = reader.ReadLine()) != null)
{

   lnCount++;
}

Console.WriteLine($"Lines Read: {lnCount}");

return 0;
