using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cactus.Cucumber
{
    public interface IConverter
    {
        public string ConvertExcelToFeatureNamed(string excelFileName, string featureFileName, bool cloakMode = false);
    }
}
