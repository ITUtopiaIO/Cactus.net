using DiffPlex;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cactus.Cucumber; // Add this using

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
        public void OutputIsAsExpected()
        {
            FileDiff fileDiff = new FileDiff();
            string outputFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.output");
            String expectedFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.exp");
            string result = fileDiff.GetFileDiff(outputFile, expectedFile);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void OutputIsNotAsExpected()
        {
            FileDiff fileDiff = new FileDiff();
            string outputFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.wrongoutput");
            String expectedFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.exp");
            string result = fileDiff.GetFileDiff(outputFile, expectedFile);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void OutputHasExtraLines()
        {
            FileDiff fileDiff = new FileDiff();
            string outputFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.outputextralines");
            String expectedFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.exp");
            string result = fileDiff.GetFileDiff(outputFile, expectedFile, ignoreEmptyLine: false);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void OutputMissingLines()
        {
            FileDiff fileDiff = new FileDiff();
            string outputFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.outputmissinglines");
            String expectedFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.exp");
            string result = fileDiff.GetFileDiff(outputFile, expectedFile, ignoreEmptyLine: false);
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public void OutputIsAsExpectedExcludingEmptyLines()
        {
            FileDiff fileDiff = new FileDiff();
            string outputFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.outputnoemptyline");
            String expectedFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.exp");
            string result = fileDiff.GetFileDiff(outputFile, expectedFile, ignoreEmptyLine:false);
            Assert.That(result, Is.Not.Empty);

            result = fileDiff.GetFileDiff(outputFile, expectedFile, ignoreEmptyLine:true);
            Assert.That(result, Is.Empty);

            //Empty line is default to be ignored
            result = fileDiff.GetFileDiff(outputFile, expectedFile);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void OutputIsAsExpectedExcludingCommentLines()
        {
            FileDiff fileDiff = new FileDiff();
            string outputFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.outputnocommentline");
            String expectedFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.exp");
            string result = fileDiff.GetFileDiff(outputFile, expectedFile);
            Assert.That(result, Is.Not.Empty);

            result = fileDiff.GetFileDiff(outputFile, expectedFile, ignoreCommentLine: true);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void SimplifyTableLineTest()
        {
            FileDiff fileDiff = new FileDiff();
            Assert.That(fileDiff.SimplifyTableLine("|"), Is.EqualTo("|"));
            Assert.That(fileDiff.SimplifyTableLine("Hello"), Is.EqualTo("Hello"));
            Assert.That(fileDiff.SimplifyTableLine("Hello|"), Is.EqualTo("Hello|"));
            Assert.That(fileDiff.SimplifyTableLine("|Hello"), Is.EqualTo("|Hello"));
            Assert.That(fileDiff.SimplifyTableLine("|Hello|"), Is.EqualTo("|Hello|"));
            Assert.That(fileDiff.SimplifyTableLine("|Hello|World|"), Is.EqualTo("|Hello|World|"));
            Assert.That(fileDiff.SimplifyTableLine("|Hello||World|"), Is.EqualTo("|Hello||World|"));
            Assert.That(fileDiff.SimplifyTableLine("| Hello | | World |"), Is.EqualTo("|Hello||World|"));

        }
    }

}
