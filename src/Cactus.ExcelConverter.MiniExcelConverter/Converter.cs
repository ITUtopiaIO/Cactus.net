using MiniExcelLibs;
using Cactus.Cucumber;
using System.Data;
using System.Dynamic;

namespace Cactus.ExcelConverter.MiniExcelConverter
{
    public class Converter : IConverter
    {
        readonly string SCENARIO = "Scenario";
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

                    DataTable table = new DataTable();
                    bool isReadingTable = false;
                    var rows = MiniExcel.Query(_excelFile, sheetName: sheetName).ToList();
                    foreach (var row in rows)
                    {
                        //If first column is empty and second column is not, then it is a table row
                        if (String.IsNullOrEmpty(Convert.ToString(row.A)) && !String.IsNullOrEmpty(Convert.ToString(row.B)))
                        {
                            //Start of table
                            if (isReadingTable == false)
                            {
                                table = new DataTable();
                                AddRowToTable(table, row, true);
                            }
                            else
                            {
                                AddRowToTable(table, row, false);
                            }

                            isReadingTable = true;
                        }
                        else
                        {
                            //End of table
                            if (isReadingTable && table != null)
                            {
                                WriteTableToFile(table, outputFile);
                                isReadingTable = false;
                                table = null;
                            }

                            //Reading non-table data
                            string rowData = string.Empty;
                            foreach (var cell in row)
                            {
                                string cellData = string.Empty;


                                if (cell.Value != null)
                                {
                                    cellData = cell.Value.ToString();

                                    if (FIRST_COLUMN.Equals(cell.Key.ToString()))
                                    {
                                        if (SCENARIO.Equals(cellData))
                                        {
                                            cellData = cellData.Trim() + COLON;
                                        }
                                        else
                                        {
                                            cellData = "\t" + cellData.Trim();
                                        }
                                    }
                                }

                                rowData += cellData.Trim() + " ";
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
                foreach (var cell in row)
                {
                    if (cell.Value != null) 
                    { 
                        table.Columns.Add(cell.Key);
                    }
                }
            }

            DataRow newRow = table.NewRow();
            foreach (var cell in row)
            {
                if (cell.Value != null)
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