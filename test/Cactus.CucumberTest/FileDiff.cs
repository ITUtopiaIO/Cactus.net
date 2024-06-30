using NUnit.Framework.Internal;
using DiffPlex;

namespace Cactus.CucumberTest
{
    public class FileDiff
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