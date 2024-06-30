using NUnit.Framework.Internal;
using DiffPlex;
using System.Text;

namespace Cactus.CucumberTest
{
    public class FileDiff
    {
        public bool FileAreSame(string outputFile, string expectedFile)
        {
            var outputLines = File.ReadAllLines(outputFile);
            StringBuilder outputData = new StringBuilder();
            foreach (string value in outputLines)
            {
                outputData.AppendLine(value);
            }

            var expectedLines = File.ReadAllLines(expectedFile);
            StringBuilder expectedData = new StringBuilder();
            foreach (string value in expectedLines)
            {
                expectedData.AppendLine(value);
            }

            Differ differ = new Differ();
            var diff = differ.CreateLineDiffs(outputData.ToString(), expectedData.ToString(), true);
            return (diff.DiffBlocks.Count == 0);
        }

    }
}