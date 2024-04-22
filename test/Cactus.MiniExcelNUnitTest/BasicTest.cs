using MiniExcelLibs;
using System.Linq;

namespace Cactus.MiniExcelNUnitTest
{
    public class BasicTest  
    {
        string _path = "TestExcelFile.xlsx";
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestReadExcelFile()
        {
            var Rows = MiniExcel.Query(_path).ToList();


            Assert.Multiple(() =>
            {
                Assert.That(Rows.Count, Is.EqualTo(2));

                Assert.That(((String)Rows[0].A).Trim(), Is.EqualTo("Test"));
                Assert.That(((String)Rows[0].B).Trim(), Is.EqualTo("Sample"));
                Assert.That(Rows[1].A, Is.EqualTo(1));
                Assert.That(Rows[1].B, Is.EqualTo(2));
            });
        }
    }
}