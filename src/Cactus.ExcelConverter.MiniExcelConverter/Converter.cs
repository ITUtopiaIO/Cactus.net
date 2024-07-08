using MiniExcelLibs;

namespace Cactus.ExcelConverter.MiniExcelConverter
{
    public class Converter
    {
        //TODO: to read mutlipe sheets

        readonly string DISCLAIM = "# This feature file was auto generated from Excel by Cactus.net(https://github.com/ITUtopiaIO/Cactus.net)";
        readonly string SCENARIO = "Scenario";
        readonly string COLON = ":";

        string _excelFile = string.Empty;
        string _featureFile = string.Empty;

        public bool ConvertExcelToFeature(string excelFileName) 
        {

            _excelFile = excelFileName;
            _featureFile = Path.ChangeExtension(excelFileName, ".feature");

            //var Rows = MiniExcel.Query(_excelFile).ToList();

            using (StreamWriter outputFile = new StreamWriter(_featureFile))
            {
                outputFile.WriteLine(DISCLAIM);
                outputFile.WriteLine();
                outputFile.WriteLine("Feature: " + Path.GetFileNameWithoutExtension(_excelFile));


                var sheetNames = MiniExcel.GetSheetNames(_excelFile);
                foreach (var sheetName in sheetNames)
                {
                    outputFile.WriteLine();
                    outputFile.WriteLine("# " + sheetName);

                    var rows = MiniExcel.Query(_excelFile, sheetName: sheetName).ToList();
                    foreach (var row in rows)
                    {
                        string rowData = string.Empty;
                        foreach (var cell in row)
                        {
                            string cellData = string.Empty;
                            if (cell.Value != null)
                            {
                                cellData = cell.Value.ToString();
                                if ("A".Equals(cell.Key.ToString()))
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
                            rowData += cellData.Trim() +" ";
                        }
                        outputFile.WriteLine(rowData);
                    }
                }
            }

            return true;
        }

        

    }
}