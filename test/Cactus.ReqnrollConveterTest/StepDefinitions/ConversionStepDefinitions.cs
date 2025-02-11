namespace Cactus.ReqnrollTest.StepDefinitions
{
    [Binding]
    public sealed class ConversionStepDefinitions
    {
        const string DEFAULT_FOLDER = "Features";

        string _excelFile = string.Empty;

        [Given("I have an Excel file named {string}")]
        public void GivenIHaveAnExcelFile(string excelFile)
        {
            GivenIHaveAnExcelFile(excelFile, DEFAULT_FOLDER);
        }

        [Given("I have an Excel file named {string} in the {string} folder")]
        public void GivenIHaveAnExcelFile(string excelFile, string folder)
        {
            if (File.Exists(Path.Combine(folder,excelFile))) 
            {
                _excelFile = Path.Combine(folder, excelFile);
            }
            else
            {
                throw new FileNotFoundException(" File not found: " + excelFile);
            }

        }

        [When("I convert the Excel file to a feature file")]
        public void WhenIConvertTheExcelFileToAFeatureFile()
        {
            ExcelConverter.Converter converter = new ExcelConverter.Converter();
            converter.ConvertExcelToFeature(_excelFile);
        }

        [When("I compare the converted feature file with {string}")]
        public void WhenICompareTheConvertedFeatureFileWith(string featureFile)
        {
            //TODO:
        }

        [Then("I should find these two files match")]
        public void ThenIShoudlFindTheseTwoFilesMatch()
        {
            //TODO:
        }
    }
}
