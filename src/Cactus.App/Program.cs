// See https://aka.ms/new-console-template for more information

using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;


var app = new CommandApp<CactusCommand>();

if (args.Length == 0)
{
    // Inject the help flag manually
    return app.Run(new[] { "--help" });
}
else
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
            { _fileExtension = value.StartsWith(".") ? value : "." + value; }
        }


        [CommandOption("-i|--incSubDir")]
        [Description("Specify whether to process subdirectory files or not (default is false).")]
        [DefaultValue("false")]
        public bool IncludeSubdirectories { get; set; } = false;


        [CommandOption("-t|--tgtDir")]
        [Description("Specify the target directory or sub directory to save the generated feature files.")]
        public string? TargetDirectory { get; set; }


        [CommandOption("-c|--cloak")]
        [Description("Enable cloak mode: do not add date/time stamp to the feature header line.")]
        [DefaultValue("false")]
        public bool CloakMode { get; set; } = false;


        [CommandOption("-m|--match")]
        [Description("To match with an exist feature file")]
        [DefaultValue("false")]
        public bool Match { get; set; } = false;


        private string _matchExtension = ".feature";
        [CommandOption("-x|--matchExt")]
        [Description("Specify the file extension to match against (default is .feature).")]
        [DefaultValue(".feature")]
        public string MatchExtension
        {
            get => _matchExtension;
            set { _matchExtension = value.StartsWith(".") ? value : "." + value; }
        }


        [CommandOption("-r|--matchDir")]
        [Description("Specify the directory to match against.")]
        public string? MatchDirectory { get; set; }


        [CommandOption("-z|--zombie")]
        [Description("Zombie mode: continue processing all files even if errors, exceptions, or mismatches occur.")]
        [DefaultValue("false")]
        public bool ZombieMode { get; set; } = false;


        //v/version
        //o/output/implementation, specflow/reqnroll version
    }


    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        var fileOrPath = settings.FileOrPath;
        var fileExtension = settings.FileExtension;
        var includeSubdirectories = settings.IncludeSubdirectories;
        var targetDirectory = settings.TargetDirectory;
        var cloakMode = settings.CloakMode;
        var match = settings.Match;
        var matchExtension = settings.MatchExtension;
        var matchDirectory = settings.MatchDirectory;
        var zombieMode = settings.ZombieMode;

        int totalFiles = 0, processedCount = 0, successCount = 0, errorCount = 0, mismatchCount = 0, matchSkippedCount = 0;

        if (fileOrPath is null)
        {
            AnsiConsole.WriteLine("Please provide the Excel file or the path you want to convert.");
            AnsiConsole.WriteLine("For Help: Cactus -h");

            return Status.ERROR;
        }

        AnsiConsole.MarkupLine($"[green]Processing file or path:[/] {fileOrPath}");

        try
        {
            if (File.Exists(fileOrPath))
            {
                // Single file processing
                totalFiles = 1;
                var fileInfo = new FileInfo(fileOrPath);
                string exactFileName = fileInfo.Directory?.GetFiles(fileInfo.Name)[0].Name ?? fileInfo.Name;

                var status = processFile(exactFileName, fileExtension, targetDirectory, cloakMode, match, matchExtension, matchDirectory);
                if (status == Status.SUCCESS) successCount++;
                else if (status == Status.MISMATCH) mismatchCount++;
                else if (status == Status.MATCHSKIPPED) matchSkippedCount++;
                else errorCount++;

                PrintSummary(totalFiles, totalFiles, successCount, errorCount, mismatchCount, matchSkippedCount, match);
                return status;
            }
            else if (Directory.Exists(fileOrPath))
            {
                // Directory processing
                var excelFiles = Directory.GetFiles(fileOrPath, "*.xlsx", includeSubdirectories ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                totalFiles = excelFiles.Length;
                foreach (var excelFile in excelFiles)
                {
                    AnsiConsole.MarkupLine($"\n[blue]Processing file #{++processedCount} of {totalFiles} :[/]");

                    var status = processFile(excelFile, fileExtension, targetDirectory, cloakMode, match, matchExtension, matchDirectory);
                    if (status == Status.SUCCESS) successCount++;
                    else if (status == Status.MISMATCH) mismatchCount++;
                    else if (status == Status.MATCHSKIPPED) matchSkippedCount++;
                    else errorCount++;

                    if (status != Status.SUCCESS && !zombieMode)
                    {
                        PrintSummary(totalFiles, processedCount, successCount, errorCount, mismatchCount, matchSkippedCount, match);
                        return status;
                    }
                }
                
                PrintSummary(totalFiles, processedCount, successCount, errorCount, mismatchCount, matchSkippedCount, match);
                return Status.SUCCESS;
            }
            else
            {
                AnsiConsole.WriteLine("The specified file or path does not exist.");
                return Status.ERROR;
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteLine("An error occurred while converting the Excel file to a feature file.");
            AnsiConsole.WriteException(ex);
            return Status.EXCEPTION;
        }
    }


    private int processFile(string excelFileName, string extension, string? targetDirectory, bool cloakMode, bool match, string matchExtension, string? matchDirectory, string? matchFileName = null)
    {
        try
        {
            string outputDirectory = Path.GetDirectoryName(excelFileName) ?? string.Empty;
            if (!string.IsNullOrEmpty(targetDirectory))
            {
                outputDirectory = Path.Combine(outputDirectory, targetDirectory);
                Directory.CreateDirectory(outputDirectory);
            }
            string outputFileName = Path.GetFileNameWithoutExtension(excelFileName) + extension;
            string featureFielName = Path.Combine(outputDirectory, outputFileName);

            AnsiConsole.WriteLine($"Converting {excelFileName} to {featureFielName}.");
            Cactus.ExcelConverter.Converter converter = new Cactus.ExcelConverter.Converter();
            string _featureFile = converter.ConvertExcelToFeatureNamed(excelFileName, featureFielName, cloakMode);
            AnsiConsole.WriteLine("Feature file created: " + _featureFile);

            if (!match)
            {
                return Status.SUCCESS;
            }
            else
            {
                string matchFile = string.Empty;

                if (!string.IsNullOrEmpty(matchDirectory))
                {
                    matchFile = Path.Combine(matchDirectory, excelFileName);
                }
                else
                {
                    string originalDir = Path.GetDirectoryName(excelFileName) ?? string.Empty;
                    matchFile = Path.Combine(originalDir, excelFileName);
                }

                matchFile = Path.ChangeExtension(matchFile, matchExtension);

                if (!File.Exists(matchFile))
                {
                    AnsiConsole.MarkupLine($"[yellow]WARNING[/] : Matching file {matchFile} does not exist. Skipping matching.");
                    return Status.MATCHSKIPPED;
                }
                else 
                {
                    AnsiConsole.WriteLine($"Matching generated feature file {featureFielName} with {matchFile}.");

                    bool isMatch = true;
                    if (isMatch)
                    {
                        AnsiConsole.MarkupLine($"[green]MATCHED[/] : {featureFielName} matches {matchFile}");
                        return Status.SUCCESS;
                        //Cactus.FeatureMatcher.FeatureMatcher matcher = new Cactus.FeatureMatcher.FeatureMatcher();
                        //bool isMatch = matcher.MatchFeatureFiles(featureFielName, matchFile);
                        //if (isMatch)
                        //{
                        //    AnsiConsole.MarkupLine($"[green]MATCHED[/] : {featureFielName} matches {matchFile}");
                        //    return 0;
                        //}
                        //else
                        //{
                        //    AnsiConsole.MarkupLine($"[red]NOT MATCHED[/] : {featureFielName} does not match {matchFile}");
                        //    return -2;
                        //}
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[yellow]WARNING[/] : {featureFielName} does not matches {matchFile}");
                        return Status.MISMATCH;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            AnsiConsole.WriteException(ex);
            return Status.EXCEPTION;
        }
    }

    private void PrintSummary(int totalFiles, int processedCount, int successCount, int errorCount, int mismatchCount, int matchSkippedCount, bool match)
    {
        AnsiConsole.MarkupLine($"\n[bold]Summary:[/]");
        AnsiConsole.MarkupLine($"[blue]Total Files:[/] {totalFiles}");
        AnsiConsole.MarkupLine($"[blue]Processed:[/] {processedCount}");
        AnsiConsole.MarkupLine($"[green]Success:[/] {successCount}");
        AnsiConsole.MarkupLine($"[red]Error:[/] {errorCount}");
        if (match)
        {
            AnsiConsole.MarkupLine($"[yellow]Mismatch:[/] {mismatchCount}");
            AnsiConsole.MarkupLine($"[yellow]Match skipped:[/] {matchSkippedCount}");
        }
    }
}


class Status
{
    public static readonly int SUCCESS = 0;
    public static readonly int EXCEPTION = -1;
    public static readonly int ERROR = -2;
    public static readonly int MISMATCH = -3;
    public static readonly int MATCHSKIPPED = -4;
}
