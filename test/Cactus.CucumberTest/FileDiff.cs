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
            var diff = differ.CreateLineDiffs(outputData.ToString(), expectedData.ToString(), true);
            if (diff.DiffBlocks.Count == 0)
            {
                return string.Empty;
            }
            else
            {
                return "It's Different";
            }
        }

    }
}