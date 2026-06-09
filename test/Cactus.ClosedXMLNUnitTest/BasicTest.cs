using ClosedXML.Excel;
using Cactus.Cucumber;
using DocumentFormat.OpenXml.Wordprocessing;

namespace Cactus.ClosedXMLNUnitTest
{
    public class BasicTest
    {
        private readonly string _file = "TestExcelFile.xlsx";

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestReadExcelFile()
        {
            using var workbook = new XLWorkbook(_file);
            var worksheet = workbook.Worksheet(1);
            var rows = worksheet.RangeUsed()!.RowsUsed().ToList();

            Assert.Multiple((System.Action)(() =>
            {
                Assert.That(rows.Count, Is.EqualTo(2));

                Assert.That(rows[0].Cell(1).GetString().Trim(), Is.EqualTo("Test"));
                Assert.That(rows[0].Cell(2).GetString().Trim(), Is.EqualTo("Sample"));
                Assert.That(rows[1].Cell(1).GetDouble(), Is.EqualTo(1d));
                Assert.That(rows[1].Cell(2).GetDouble(), Is.EqualTo(2d));
            }));
        }

        [Test]
        public void TestReadExcelSheet()
        {
            using var workbook = new XLWorkbook(_file);

            foreach (var worksheet in workbook.Worksheets)
            {
                var rows = worksheet.RangeUsed()!.RowsUsed().ToList();

                if (worksheet.Name == "Sheet2")
                {
                    Assert.Multiple((System.Action)(() =>
                    {
                        Assert.That(rows.Count, Is.EqualTo(2));

                        Assert.That(rows[0].Cell(1).GetString().Trim(), Is.EqualTo("Another"));
                        Assert.That(rows[0].Cell(2).GetString().Trim(), Is.EqualTo("Sheet"));
                        Assert.That(rows[1].Cell(1).GetString().Trim(), Is.EqualTo("A"));
                        Assert.That(rows[1].Cell(2).GetString().Trim(), Is.EqualTo("S"));
                    }));
                }
            }
        }

        [Test]
        public void TestReadExcelDateTime()
        {
            using var workbook = new XLWorkbook(_file);

            foreach (var worksheet in workbook.Worksheets)
            {
                var rows = worksheet.RangeUsed()!.RowsUsed().ToList();

                if (worksheet.Name == "DateTime")
                {
                    Assert.Multiple((System.Action)(() =>
                    {
                        var cell = worksheet.Cell(1, 1);
                        Assert.That(cell.DataType, Is.EqualTo(XLDataType.DateTime));
                        string formattedDateTime = Utility.FormatDateTime(cell.GetDateTime());
                        Assert.That(formattedDateTime, Is.EqualTo(Utility.FormatDateTime(new DateTime(2026, 7, 4, 0, 0, 0))));

                        cell = worksheet.Cell(2, 1);
                        Assert.That(cell.DataType, Is.EqualTo(XLDataType.DateTime));
                        formattedDateTime = Utility.FormatDateTime(cell.GetDateTime());
                        Assert.That(formattedDateTime, Is.EqualTo(Utility.FormatDateTime(new DateTime(2026, 7, 1, 0, 0, 0))));

                        cell = worksheet.Cell(3, 1);
                        Assert.That(cell.DataType, Is.EqualTo(XLDataType.DateTime));
                        formattedDateTime = Utility.FormatDateTime(cell.GetDateTime());
                        Assert.That(formattedDateTime, Is.EqualTo(Utility.FormatDateTime(new DateTime(2026, 7, 1, 0, 0, 0))));

                        //NOTE: Custom formated DateTime is treated as number at this point, need reformate the spreadsheet as date first to process
                        cell = worksheet.Cell(4, 1);
                        Assert.That(cell.DataType, Is.EqualTo(XLDataType.Number));
                    }));
                }
            }
        }
    }
}
