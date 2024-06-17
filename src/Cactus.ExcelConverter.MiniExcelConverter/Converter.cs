using MiniExcelLibs;

namespace Cactus.ExcelConverter.MiniExcelConverter
{
    public class Converter
    {
        //TODO: to read mutlipe sheets

        string _excelFile = string.Empty;
        string _featureFile = string.Empty;

        public bool ConvertExcelToFeature(string excelFileName) 
        {

            _excelFile = excelFileName;
            _featureFile = excelFileName+".feature";

            var Rows = MiniExcel.Query(_excelFile).ToList();

            using (StreamWriter outputFile = new StreamWriter(_featureFile))
            {
                outputFile.WriteLine("Feature: " + _excelFile);
            }

                return true;
        }

        

    }
}