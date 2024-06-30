using NUnit.Framework.Internal;
using DiffPlex;

namespace Cactus.CucumberTest
{
    public class FileDiff
    {
        public bool FileAreSame(string outputFile, string expectedFile)
        {
            string output = File.ReadAllText(outputFile);
            string expected = File.ReadAllText(expectedFile);

            Differ differ = new Differ();
            var diff = differ.CreateLineDiffs(output, expected, true);
            return (diff.DiffBlocks.Count == 0);
        }

    }
}