using MiniExcelLibs;
using System.IO;
using System.Linq;

namespace Cactus.MiniExcelNUnitTest
{
    public class BasicTest  
    {
        string _file = "TestExcelFile.xlsx";
        
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestReadExcelFile()
        {
            var Rows = MiniExcel.Query(_file).ToList();


            Assert.Multiple(() =>
            {
                Assert.That(Rows.Count, Is.EqualTo(2));

                Assert.That(((String)Rows[0].A).Trim(), Is.EqualTo("Test"));
                Assert.That(((String)Rows[0].B).Trim(), Is.EqualTo("Sample"));
                Assert.That(Rows[1].A, Is.EqualTo(1));
                Assert.That(Rows[1].B, Is.EqualTo(2));
            });
        }

        [Test]
        public void TestReadExcelSheet()
        {

            var sheetNames = MiniExcel.GetSheetNames(_file);
            foreach (var sheetName in sheetNames)
            {
                var rows = MiniExcel.Query(_file, sheetName: sheetName).ToList();

                if (sheetName == "Sheet2")
                {
                    Assert.Multiple(() =>
                    {
                        Assert.That(rows.Count, Is.EqualTo(2));

                        Assert.That(((String)rows[0].A).Trim(), Is.EqualTo("Another"));
                        Assert.That(((String)rows[0].B).Trim(), Is.EqualTo("Sheet"));
                        Assert.That(((String)rows[1].A).Trim(), Is.EqualTo("A"));
                        Assert.That(((String)rows[1].B).Trim(), Is.EqualTo("S"));
                    });
                }
            }
           
        }

    }
}