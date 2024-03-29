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
        public void Test1()
        {
            var Rows = MiniExcel.Query(_path).ToList();

            Assert.That(Rows.Count, Is.EqualTo(2));
        }
    }
}