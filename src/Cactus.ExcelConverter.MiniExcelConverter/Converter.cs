using MiniExcelLibs;

namespace Cactus.ExcelConverter.MiniExcelConverter
{
    public class Converter
    {
        //TODO: to read mutlipe sheets

        string _file = string.Empty;

        public bool ConvertToFeature(string FileName) 
        {
                      
            _file = FileName;

            var Rows = MiniExcel.Query(_file).ToList();

            return true;
        }

        

    }
}