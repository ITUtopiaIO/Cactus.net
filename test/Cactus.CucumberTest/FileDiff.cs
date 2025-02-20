using NUnit.Framework.Internal;
using DiffPlex;
using System.Text;

namespace Cactus.CucumberTest
{
    public class FileDiff
    {
        public string GetFileDiff(string outputFile, string expectedFile, bool ignoreEmptyLine=true, bool ignoreCommentLine=false)
        {
            var outputLines = File.ReadAllLines(outputFile);
            StringBuilder outputData = new StringBuilder();
            foreach (string value in outputLines)
            {
                if (ignoreEmptyLine && string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }
                if (ignoreCommentLine && value.Trim().StartsWith("#"))
                {
                    continue;
                }
                outputData.AppendLine(value);
            }

            var expectedLines = File.ReadAllLines(expectedFile);
            StringBuilder expectedData = new StringBuilder();
            foreach (string value in expectedLines)
            {
                if (ignoreEmptyLine && string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }
                if (ignoreCommentLine && value.Trim().StartsWith("#"))
                {
                    continue;
                }
                expectedData.AppendLine(value);
            }

            Differ differ = new Differ();
            var diff = differ.CreateLineDiffs(expectedData.ToString(), outputData.ToString(), true);
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

    }
}