using NUnit.Framework.Internal;
using DiffPlex;
using System.Text;
using Cactus.Cucumber;

namespace Cactus.CucumberTest
{
    public class FileDiff
    {
        public string GetFileDiff(string outputFile, string expectedFile, bool ignoreEmptyLine=true, bool ignoreCommentLine=false)
        {
            string outputData = ReadFileData(outputFile, ignoreEmptyLine, ignoreCommentLine);
            string expectedData = ReadFileData(expectedFile, ignoreEmptyLine, ignoreCommentLine);

            Differ differ = new Differ();
            var diff = differ.CreateLineDiffs(expectedData, outputData, true);
            if (diff.DiffBlocks.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                int padLengh = 15;
                int showDiffCount = 5;
                StringBuilder diffData = new StringBuilder();
                diffData.AppendLine(diff.DiffBlocks.Count+" difference(s) found:");
                for (int i = 1; i <= diff.DiffBlocks.Count && i<= showDiffCount; i++)
                {
                    var diffBlock = diff.DiffBlocks[i-1];
                    diffData.AppendLine("Difference " + (i) + ":");
                    diffData.AppendLine("Expected: ".PadLeft(padLengh) + diff.PiecesOld[diffBlock.DeleteStartA].Trim());
                    for (int j = 2; j <= diffBlock.DeleteCountA && j<=showDiffCount; j++)
                    {
                        diffData.AppendLine("".PadLeft(padLengh) + diff.PiecesOld[diffBlock.DeleteStartA+j-1].Trim());

                        if (j == showDiffCount && j < diffBlock.DeleteCountA)
                        {
                            diffData.AppendLine("".PadLeft(padLengh) + "...");
                        }
                    }

                    diffData.AppendLine("Actual: ".PadLeft(padLengh) + diff.PiecesNew[diffBlock.InsertStartB].Trim());
                    for (int j = 2; j <= diffBlock.InsertCountB && j <= showDiffCount; j++)
                    {
                        diffData.AppendLine("".PadLeft(padLengh) + diff.PiecesNew[diffBlock.InsertStartB+j-1].Trim());

                        if (j == showDiffCount && j < diffBlock.InsertCountB)
                        {
                            diffData.AppendLine("".PadLeft(padLengh)+"...");
                        }
                    }

                    if (i==showDiffCount && i < diff.DiffBlocks.Count)
                    {
                        diffData.AppendLine("More Differences ...");
                    }

                }
                return diffData.ToString();
            }
        }

        private string ReadFileData(string fileName, bool ignoreEmptyLine, bool ignoreCommentLine)
        {
            var lines = File.ReadAllLines(fileName);
            StringBuilder sb = new StringBuilder();
            foreach (string value in lines)
            {
                if (value.Trim().StartsWith(Cucumber.Common.DISCLAIM))
                {
                    continue;
                }
                if (ignoreEmptyLine && string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }
                if (ignoreCommentLine && value.Trim().StartsWith("#"))
                {
                    continue;
                }
                if (value.Trim().StartsWith(Cucumber.Common.TABLEDIV))
                {
                    sb.Append(SimplifyTableLine(value));
                    continue;
                }

                sb.AppendLine(value);
            }

            return sb.ToString();
        }

        internal string SimplifyTableLine(string tableLine)
        {

            StringBuilder sb = new StringBuilder();
            string tempTableLine = tableLine.Trim();
            if (tempTableLine.Length< 2 || 
                !tempTableLine.StartsWith(Cucumber.Common.TABLEDIV) ||
                !tempTableLine.EndsWith(Cucumber.Common.TABLEDIV))
            {
                return tableLine;
            }
            else
            {
                tempTableLine = tempTableLine.Substring(1, tempTableLine.Length - 2);
            }

            string[] cols = tempTableLine.Split(Cucumber.Common.TABLEDIV, StringSplitOptions.TrimEntries);
            foreach (string col in cols)
            {
                sb.Append(Cucumber.Common.TABLEDIV+col.Trim());
            }
            sb.Append(Cucumber.Common.TABLEDIV);

            return sb.ToString();
        }

    }
}