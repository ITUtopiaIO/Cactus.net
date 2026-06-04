using Cactus.Cucumber;
using ClosedXML.Excel;
using System.Data;
using System.Globalization;

namespace Cactus.ExcelConverter.ClosedXMLConverter
{
    public class Converter : IConverter
    {
        readonly string SCENARIO = "Scenario";
        readonly string SCENARIO_OUTLINE = "Scenario Outline";
        readonly string EXAMPLES = "Examples";
        readonly string BACKGROUND = "Background";
        readonly string COLON = ":";

        string _excelFile = string.Empty;
        string _featureFile = string.Empty;

        public string ConvertExcelToFeatureNamed(string excelFileName, string featureFileName, bool cloakMode = false)
        {
            _excelFile = excelFileName;
            _featureFile = featureFileName;

            using StreamWriter outputFile = new StreamWriter(_featureFile);

            string disclaimer = Cucumber.Common.DISCLAIM;

            if (!cloakMode)
            {
                disclaimer += " at " + DateTime.Now.ToString();
            }
            outputFile.WriteLine(disclaimer);
            outputFile.WriteLine();

            outputFile.WriteLine("Feature: " + Path.GetFileNameWithoutExtension(_excelFile));

            using XLWorkbook workbook = new XLWorkbook(_excelFile);
            foreach (var worksheet in workbook.Worksheets)
            {
                string sheetName = worksheet.Name;

                outputFile.WriteLine();
                outputFile.WriteLine("#Sheet: " + sheetName);

                if (sheetName.StartsWith("_"))
                {
                    continue;
                }

                if (sheetName.Equals(BACKGROUND))
                {
                    outputFile.WriteLine(BACKGROUND + COLON);
                }

                DataTable? table = new DataTable();
                bool isReadingTable = false;

                var usedRange = worksheet.RangeUsed();
                if (usedRange == null)
                {
                    continue;
                }

                int firstRow = usedRange.RangeAddress.FirstAddress.RowNumber;
                int lastRow = usedRange.RangeAddress.LastAddress.RowNumber;

                for (int rowIndex = firstRow; rowIndex <= lastRow; rowIndex++)
                {
                    bool emptyRow = IsRowEmpty(worksheet, rowIndex);
                    string firstColumn = GetCellData(worksheet.Cell(rowIndex, 1));
                    string secondColumn = GetCellData(worksheet.Cell(rowIndex, 2));

                        //If first column is empty and second column is not, then it is a table header row.
                    if (!isReadingTable && string.IsNullOrEmpty(firstColumn) && !string.IsNullOrEmpty(secondColumn))
                    {
                        table = new DataTable();
                        AddRowToTable(table, worksheet, rowIndex, true);
                        isReadingTable = true;
                    }
                    else if (isReadingTable && string.IsNullOrEmpty(firstColumn) && !emptyRow)
                    {
                        AddRowToTable(table!, worksheet, rowIndex, false);
                    }
                    else if (isReadingTable && !string.IsNullOrEmpty(firstColumn) && firstColumn.StartsWith("#"))
                    {
                            //Comment row inside table, will be ignored and not added to the table
                            //Potential issue: if the comment row is at the end of the table, it actually should be included in the feature file, will review this one later.
                        continue;
                    }
                    else
                    {
                            //End of table if there's an empty row or non-table row
                        if (isReadingTable && table != null)
                        {
                            WriteTableToFile(table, outputFile);
                            isReadingTable = false;
                            table = null;
                        }

                            //Reading non-table data
                            //If both first and second column are empty, then this row will be ignored
                        if (string.IsNullOrEmpty(firstColumn) && string.IsNullOrEmpty(secondColumn))
                        {
                            continue;
                        }

                        string rowData = string.Empty;
                        int emptyCellCount = 0;
                        int lastColumn = usedRange.RangeAddress.LastAddress.ColumnNumber;

                        for (int columnIndex = 1; columnIndex <= lastColumn; columnIndex++)
                        {
                            var cell = worksheet.Cell(rowIndex, columnIndex);
                            string cellData = string.Empty;

                            if (!cell.IsEmpty() && emptyCellCount < 2)
                            {
                                emptyCellCount = 0;

                                cellData = GetCellData(cell);

                                if (columnIndex == 1)
                                {
                                    if (SCENARIO.Equals(cellData) ||
                                        SCENARIO_OUTLINE.Equals(cellData) ||
                                        EXAMPLES.Equals(cellData))
                                    {
                                        cellData = cellData.Trim() + COLON;
                                    }
                                    else
                                    {
                                        cellData = "\t" + cellData.Trim();
                                    }
                                }

                                rowData += cellData.Trim() + " ";
                            }
                            else
                            {
                                emptyCellCount++;
                            }
                        }

                        outputFile.WriteLine(rowData);
                    }
                }

                    //If table is at the end of the sheet
                if (isReadingTable && table != null)
                {
                    WriteTableToFile(table, outputFile);
                    isReadingTable = false;
                }
            }

            return _featureFile;
        }

        private bool AddRowToTable(DataTable table, IXLWorksheet worksheet, int rowIndex, bool isHeadRow)
        {
            if (isHeadRow)
            {
                int lastColumn = worksheet.RangeUsed()!.RangeAddress.LastAddress.ColumnNumber;
                for (int columnIndex = 2; columnIndex <= lastColumn; columnIndex++)
                {
                    var cell = worksheet.Cell(rowIndex, columnIndex);
                    if (!cell.IsEmpty())
                    {
                        table.Columns.Add(cell.Address.ColumnLetter);
                    }
                    else
                    {
                        break;
                    }
                }
            }

            bool hasData = false;
            DataRow newRow = table.NewRow();
            foreach (DataColumn column in table.Columns)
            {
                var cell = worksheet.Cell(rowIndex, XLHelper.GetColumnNumberFromLetter(column.ColumnName));
                if (!cell.IsEmpty())
                {
                    hasData = true;
                    newRow[column.ColumnName] = GetCellData(cell);
                }
            }

            if (hasData)
            {
                table.Rows.Add(newRow);
                return true;
            }

            return false;
        }

        private void WriteTableToFile(DataTable table, StreamWriter outputFile)
        {
            foreach (DataColumn column in table.Columns)
            {
                int width = 0;
                foreach (DataRow row in table.Rows)
                {
                    if (row[column].ToString()!.Length > width)
                    {
                        width = row[column].ToString()!.Length;
                    }
                }
                column.ExtendedProperties.Add("Width", width);

                string paddingDirection = "Right";
                foreach (DataRow row in table.Rows)
                {
                    if (double.TryParse(row[column].ToString(), out _))
                    {
                        paddingDirection = "Left";
                        break;
                    }
                }
                column.ExtendedProperties.Add("PaddingDirection", paddingDirection);
            }

            for (int rowIndex = 0; rowIndex < table.Rows.Count; rowIndex++)
            {
                DataRow row = table.Rows[rowIndex];
                string rowData = string.Empty;
                foreach (DataColumn column in table.Columns)
                {
                    if (rowIndex == 0 || "Right".Equals(column.ExtendedProperties["PaddingDirection"]?.ToString()))
                    {
                        rowData += Cucumber.Common.TABLEDIV + " " + row[column].ToString()!.Trim().PadRight(int.Parse(column.ExtendedProperties["Width"]!.ToString()!), ' ') + " ";
                    }
                    else
                    {
                        rowData += Cucumber.Common.TABLEDIV + " " + row[column].ToString()!.Trim().PadLeft(int.Parse(column.ExtendedProperties["Width"]!.ToString()!), ' ') + " ";
                    }
                }
                rowData += Cucumber.Common.TABLEDIV;

                outputFile.WriteLine(rowData);
            }
        }

        private static bool IsRowEmpty(IXLWorksheet worksheet, int rowIndex)
        {
            var usedRange = worksheet.RangeUsed();
            if (usedRange == null)
            {
                return true;
            }

            int lastColumn = usedRange.RangeAddress.LastAddress.ColumnNumber;
            for (int columnIndex = 1; columnIndex <= lastColumn; columnIndex++)
            {
                if (!worksheet.Cell(rowIndex, columnIndex).IsEmpty())
                {
                    return false;
                }
            }

            return true;
        }

        private static string GetCellData(IXLCell cell)
        {
            if (cell.IsEmpty())
            {
                return string.Empty;
            }

            if (cell.DataType == XLDataType.DateTime)
            {
                return cell.GetDateTime().ToString("M/d/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            }

            if (cell.DataType == XLDataType.Number)
            {
                var d = cell.GetDouble();
                return RemoveTrailingZeros(d.ToString("0.#############################", CultureInfo.InvariantCulture));
            }

            return RemoveTrailingZeros(cell.GetFormattedString());
        }

        private static string RemoveTrailingZeros(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            int dotIndex = value.IndexOf('.');
            if (dotIndex <= 0 || dotIndex == value.Length - 1)
            {
                return value;
            }

            int startIndex = value[0] == '-' || value[0] == '+' ? 1 : 0;
            for (int i = startIndex; i < value.Length; i++)
            {
                if (i == dotIndex)
                {
                    continue;
                }

                if (!char.IsDigit(value[i]))
                {
                    return value;
                }
            }

            int endIndex = value.Length - 1;
            while (endIndex > dotIndex && value[endIndex] == '0')
            {
                endIndex--;
            }

            if (endIndex == dotIndex)
            {
                endIndex--;
            }

            return value.Substring(0, endIndex + 1);
        }
    }
}
