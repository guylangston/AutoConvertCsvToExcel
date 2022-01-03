# AutoConvertCsvToExcel [Work In Progress, Not Working yet]
> Opening an `.CSV` (comma seperated values) file will automatically convert the file to Microsoft Excel `.XLSX`, then open the new file in Excel.

Excel users often download `.CSV` files from the internet (from online banking, etc), then make changes to the file. When Excel saves it is very wasy to loose all of your work by mistake.

## How it works?

- Remaps `.CSV` files to this utility
- Double-clicking / Opening the `.CSV` file will automatically convert the file to excel, and
- Replace the Inputfile with and `.XLSX` file of the same name
- Opens the new `.XLSX` file in Excel

## Features:
- **Transparent** no popups, just a small notification
- Watch `.CSV` Windows file associations. Remap to this tool

## Typical Scenario
- Download `statement.csv` from your online banking website
- Double-clicking `statement.csv` from Explorer to open in Excel
- Runs `AutoConvertCsvToExcel` which:
  - imports `statement.csv`
  - converts it `.XLSX`
  - writes `statement.xlsx`
  - writes `statement-converted.csv`
- Opens `statement.xlsx` in Excel
