using DiffPlex;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus.CucumberTest
{
    internal class FileDiffTest
    {
        readonly string DEFAULT_FOLDER = "Files";


        [SetUp]
        public void Setup()
        {
        }


        [Test]
        public void Test2()
        {
            FileDiff fileDiff = new FileDiff();
            string outputFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.output");
            String expectedFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.exp");
            var result = fileDiff.FileAreSame(outputFile, expectedFile);
            Assert.True(result);
        }

        [Test]
        public void Test1()
        {
            Differ differ = new Differ();
            var diff = differ.CreateLineDiffs("Hello   world!", "Hello world!", true);
            Assert.NotNull(diff);
            Assert.That(diff.DiffBlocks.Count, Is.EqualTo(1));
        }
    }


}
