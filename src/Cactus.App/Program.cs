// See https://aka.ms/new-console-template for more information

using Spectre.Console;
using Spectre.Console.Cli;
using System.Runtime.CompilerServices;

int numberOfArguments = args.Length;
if (numberOfArguments == 0)
{
    AnsiConsole.WriteLine("Please provide the path to the Excel file you want to convert.");
    return;
}

string _excelFile = args[0];

try
{
    Cactus.ExcelConverter.Converter converter = new Cactus.ExcelConverter.Converter();
    String _featureFile = converter.ConvertExcelToFeature(_excelFile);
    AnsiConsole.WriteLine("Feature file created: " + _featureFile);

}
catch (Exception ex)
{
    AnsiConsole.WriteLine("An error occurred while converting the Excel file to a feature file.");
    AnsiConsole.WriteException(ex);
    return;
}




