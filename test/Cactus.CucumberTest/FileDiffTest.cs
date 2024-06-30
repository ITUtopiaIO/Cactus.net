using DiffPlex;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus.CucumberTest
{
    internal class FileDiffTest
    {

        [SetUp]
        public void Setup()
        {
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
