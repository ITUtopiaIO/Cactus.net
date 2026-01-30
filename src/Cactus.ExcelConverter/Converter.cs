using Cactus.ExcelConverter.MiniExcelConverter;
using System.Runtime.Serialization;
using Cactus.Cucumber;


namespace Cactus.ExcelConverter
{
    public class Converter
    {
        IConverter converter;

        public Converter() 
        {
            converter = new MiniExcelConverter.Converter();
        }

        public string ConvertExcelToFeature(string excelFileName, string extension=".feature")
        {
            string _featureFile = Path.ChangeExtension(excelFileName, extension);

            return this.ConvertExcelToFeatureNamed(excelFileName, _featureFile);
        }

        public string ConvertExcelToFeatureNamed(string excelFileName, string featureFileName)
        {
            if (!File.Exists(excelFileName))
            {
                throw new FileNotFoundException("The specified Excel file does not exist: " + excelFileName);
            }

            return converter.ConvertExcelToFeatureNamed(excelFileName, featureFileName);
        }
    }


}