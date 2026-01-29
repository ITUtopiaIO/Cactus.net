// See https://aka.ms/new-console-template for more information

using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Runtime.CompilerServices;

var app = new CommandApp<CactusCommand>();

return await app.RunAsync(args);


public class CactusCommand : AsyncCommand<CactusCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[FileOrPath]")]
        public string? FileOrPath { get; set; }


        private string _fileExtension = ".feature";
        [CommandOption("-e|--ext")]
        [Description("Specify the file extension of the generated feature file (default is .feature).")]
        [DefaultValue("feature")]
        public string FileExtension 
        { 
            get => _fileExtension;
            set
            { _fileExtension = value.StartsWith(".") ? value: "."+value; } 
        }

        [CommandOption("-d|--IncSubDir")]
        [Description("Specify whether to process subdirectory files or not (default is false).")]
        [DefaultValue("false")]
        public bool IncludeSubdirectories { get; set; } = false;
    }


    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var fileOrPath = settings.FileOrPath;
        var fileExtension = settings.FileExtension;
        var includeSubdirectories = settings.IncludeSubdirectories;

        if (fileOrPath is null)
        {
            AnsiConsole.WriteLine("Please provide the path to the Excel file you want to convert.");
            AnsiConsole.WriteLine("For Help: Cactus -h");
            return -1;
        }

        AnsiConsole.MarkupLine($"[green]Processing file or path:[/] {fileOrPath}");

        try
        {
            if (File.Exists(fileOrPath))
            {
                var fileInfo = new FileInfo(fileOrPath);
                string exactFileName = fileInfo.Directory?.GetFiles(fileInfo.Name)[0].Name ?? fileInfo.Name;

                ConvertExcelToFeature(exactFileName, fileExtension);
                
            }
            else if (Directory.Exists(fileOrPath))
            {
                SearchOption searchOption = includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

                var excelFiles = Directory.GetFiles(fileOrPath, "*.xlsx", searchOption);

                foreach (var excelFile in excelFiles)
                {
                    ConvertExcelToFeature(excelFile, fileExtension);
                }
            }
            else
            {
                AnsiConsole.WriteLine("The specified file or path does not exist.");
                return -1;
            }

            return 0;
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine("An error occurred while converting the Excel file to a feature file.");
            AnsiConsole.WriteException(ex);
            return -2;
        }
    }

    private void ConvertExcelToFeature(string excelFileName, string extension)
    {
        AnsiConsole.WriteLine("Converting " + excelFileName +" to a feature file.");
        Cactus.ExcelConverter.Converter converter = new Cactus.ExcelConverter.Converter();
        String _featureFile = converter.ConvertExcelToFeature(excelFileName, extension);
        AnsiConsole.WriteLine("Feature file created: " + _featureFile);
    }
}

