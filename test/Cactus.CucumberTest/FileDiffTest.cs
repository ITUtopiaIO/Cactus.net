﻿using DiffPlex;
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
        public void OutputIsAsExpected()
        {
            FileDiff fileDiff = new FileDiff();
            string outputFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.output");
            String expectedFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.exp");
            var result = fileDiff.FileAreSame(outputFile, expectedFile);
            Assert.True(result);
        }

        [Test]
        public void OutputIsNotAsExpected()
        {
            FileDiff fileDiff = new FileDiff();
            string outputFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.wrongoutput");
            String expectedFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.exp");
            var result = fileDiff.FileAreSame(outputFile, expectedFile);
            Assert.False(result);
        }

        [Test]
        public void OutputIsAsExpectedExcludingEmptyLines()
        {
            FileDiff fileDiff = new FileDiff();
            string outputFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.outputnoemptyline");
            String expectedFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.exp");
            var result = fileDiff.FileAreSame(outputFile, expectedFile, ignoreEmptyLine:false);
            Assert.False(result);

            result = fileDiff.FileAreSame(outputFile, expectedFile, ignoreEmptyLine:true);
            Assert.True(result);

            //Empty line is default to be ignored
            result = fileDiff.FileAreSame(outputFile, expectedFile);
            Assert.True(result);
        }

        [Test]
        public void OutputIsAsExpectedExcludingCommentLines()
        {
            FileDiff fileDiff = new FileDiff();
            string outputFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.outputnocommentline");
            String expectedFile = Path.Combine(DEFAULT_FOLDER, "SampleTest.exp");
            var result = fileDiff.FileAreSame(outputFile, expectedFile);
            Assert.False(result);

            result = fileDiff.FileAreSame(outputFile, expectedFile, ignoreCommentLine: true);
            Assert.True(result);
        }
    }


}
