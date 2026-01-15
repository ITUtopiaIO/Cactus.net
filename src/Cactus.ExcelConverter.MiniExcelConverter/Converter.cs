using MiniExcelLibs;
using Cactus.Cucumber;
using System.Data;
using System.Dynamic;

namespace Cactus.ExcelConverter.MiniExcelConverter
{
    public class Converter : IConverter
    {
        readonly string SCENARIO = "Scenario";
        readonly string SCENARIO_OUTLINE = "Scenario Outline";
        readonly string EXAMPLES = "Examples";
        readonly string BACKGROUND = "Background";
        readonly string COLON = ":";
        readonly string FIRST_COLUMN = "A";
        readonly string SECOND_COLUMN = "B";

        string _excelFile = string.Empty;
        string _featureFile = string.Empty;

        public string ConvertExcelToFeature(string excelFileName) 
        {

            _excelFile = excelFileName;
            _featureFile = Path.ChangeExtension(excelFileName, ".feature");

            using (StreamWriter outputFile = new StreamWriter(_featureFile))
            {
                outputFile.WriteLine(Cucumber.Common.DISCLAIM+" at "+DateTime.Now.ToString());
                outputFile.WriteLine();
                outputFile.WriteLine("Feature: " + Path.GetFileNameWithoutExtension(_excelFile));


                var sheetNames = MiniExcel.GetSheetNames(_excelFile);
                foreach (var sheetName in sheetNames)
                {
                    outputFile.WriteLine();
                    outputFile.WriteLine("#Sheet: " + sheetName);

                    if (sheetName.StartsWith("_"))
                    {
                        continue;
                    }

                    if (sheetName.Equals(BACKGROUND))
                    {
                        outputFile.WriteLine(BACKGROUND+ COLON);
                    }

                    DataTable table = new DataTable();
                    bool isReadingTable = false;
                    var rows = MiniExcel.Query(_excelFile, sheetName: sheetName).ToList();
                    foreach (var row in rows)
                    {
                        bool emptyRow = true;
                        foreach (var cell in row)
                        {
                            if (cell.Value != null)
                            {
                                emptyRow = false;
                                break;
                            }
                        }

                        //If first column is empty and second column is not, then it is a table header row.
                        if (!isReadingTable && String.IsNullOrEmpty(Convert.ToString(row.A)) && !String.IsNullOrEmpty(Convert.ToString(row.B)))
                        {
                            table = new DataTable();
                            AddRowToTable(table, row, true);
                            isReadingTable = true;
                        }
                        else if (isReadingTable & !emptyRow)
                        {
                            AddRowToTable(table, row, false);
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
                            if (String.IsNullOrEmpty(Convert.ToString(row.A)) && String.IsNullOrEmpty(Convert.ToString(row.B)))
                            {
                                continue;
                            }

                            string rowData = string.Empty;
                            int emptyCellCount = 0;
                            foreach (var cell in row)
                            {
                                string cellData = string.Empty;

                                if (cell.Value != null && emptyCellCount < 2)
                                {
                                    emptyCellCount = 0;
                                    cellData = cell.Value.ToString();

                                    if (FIRST_COLUMN.Equals(cell.Key.ToString()))
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
                    if (isReadingTable && table !=null)
                    {
                        WriteTableToFile(table, outputFile);

                        isReadingTable = false;
                    }
                }
            }

            return _featureFile;
        }


        private void AddRowToTable(DataTable table, ExpandoObject row, bool isHeadRow)
        {
            if (isHeadRow)
            {
                for (int i = 1; i < row.Count(); i++)
                {
                    if (row.ElementAt(i).Value != null)
                    {
                        table.Columns.Add(row.ElementAt(i).Key);
                    }
                    else
                        break;
                }
            }

            DataRow newRow = table.NewRow();
            foreach (var cell in row)
            {
                if (cell.Value != null && table.Columns.Contains(cell.Key))
                { 
                    newRow[cell.Key.ToString()] = cell.Value;
                }
            }
            table.Rows.Add(newRow);
        }


        private void WriteTableToFile(DataTable table, StreamWriter outputFile)
        {
            foreach (DataColumn column in table.Columns)
            {
                int width = 0;
                foreach (DataRow row in table.Rows)
                {
                    if (row[column].ToString().Length > width)
                    {
                        width= row[column].ToString().Length;
                    }
                }
                column.ExtendedProperties.Add("Width", width);

                string paddingDirection = "Right";
                foreach (DataRow row in table.Rows)
                {
                    double num;
                    if ( double.TryParse(row[column].ToString(), out num))
                    {
                        paddingDirection = "Left";
                        break;
                    }
                }
                column.ExtendedProperties.Add("PaddingDirection", paddingDirection);
            }

            for (int rowIndex =0; rowIndex < table.Rows.Count; rowIndex++)
            {
                DataRow row = table.Rows[rowIndex];
                string rowData = string.Empty;
                foreach (DataColumn column in table.Columns)
                {
                    if (rowIndex == 0 ||
                        "Right".Equals(column.ExtendedProperties["PaddingDirection"].ToString()))
                    {
                        rowData += Cucumber.Common.TABLEDIV + " " + row[column].ToString().Trim().PadRight(Int32.Parse(column.ExtendedProperties["Width"].ToString()), ' ') + " ";
                    }
                    else
                    {
                        rowData += Cucumber.Common.TABLEDIV + " " + row[column].ToString().Trim().PadLeft(Int32.Parse(column.ExtendedProperties["Width"].ToString()), ' ') + " ";
                    }
                }
                rowData += Cucumber.Common.TABLEDIV;

                outputFile.WriteLine(rowData);
            }
        }

    }
}