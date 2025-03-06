using MiniExcelLibs;
using Cactus.Cucumber;

namespace Cactus.ExcelConverter.MiniExcelConverter
{
    public class Converter : IConverter
    {
        //TODO: to read mutlipe sheets

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

            //var Rows = MiniExcel.Query(_excelFile).ToList();

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

                    var rows = MiniExcel.Query(_excelFile, sheetName: sheetName).ToList();
                    foreach (var row in rows)
                    {
                        string rowData = string.Empty;

                        bool isTableRow = false;
                        if (String.IsNullOrEmpty(Convert.ToString(row.A)) && !String.IsNullOrEmpty(Convert.ToString(row.B)))
                        {
                            isTableRow = true;
                        }

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

                                //Table data will not start w/ first column
                                if (isTableRow)
                                {
                                    cellData = "| " + cellData.Trim() + " ";
                                }   
                            }

                            rowData += cellData.Trim() + " ";
                        }

                        if (isTableRow)
                        {
                            rowData += " |";
                        }
                        outputFile.WriteLine(rowData);
                    }
                }
            }

            return _featureFile;
        }

        

    }
}