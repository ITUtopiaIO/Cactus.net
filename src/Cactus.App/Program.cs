// See https://aka.ms/new-console-template for more information

int numberOfArguments = args.Length;
if (numberOfArguments == 0)
{
    Console.WriteLine("Please provide the path to the Excel file you want to convert.");
    return;
}

string _excelFile = args[0];

try
{
    Cactus.ExcelConverter.Converter converter = new Cactus.ExcelConverter.Converter();
    String _featureFile = converter.ConvertExcelToFeature(_excelFile);
    Console.WriteLine("Feature file created: " + _featureFile);

}
catch (Exception ex)
{
    Console.WriteLine("An error occurred while converting the Excel file to a feature file.");
    Console.WriteLine(ex.ToString());
    Console.WriteLine(ex.Message);
    return;
}


