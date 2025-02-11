using Cactus.ExcelConverter.MiniExcelConverter;


namespace Cactus.ExcelConverter
{
    public class Converter
    {
        public bool ConvertExcelToFeature(string excelFileName)
        {

            MiniExcelConverter.Converter miniExcelConverter = new MiniExcelConverter.Converter();
            return miniExcelConverter.ConvertExcelToFeature(excelFileName);
        }
    }


}