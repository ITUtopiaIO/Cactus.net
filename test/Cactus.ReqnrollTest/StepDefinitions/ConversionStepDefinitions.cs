namespace Cactus.ReqnrollTest.StepDefinitions
{
    [Binding]
    public sealed class ConversionStepDefinitions
    {
        const string DEFAULT_FOLDER = "Features";


        [Given("I have an Excel file named {string}")]
        public void GivenIHaveAnExcelFile(string excelFile)
        {
            GivenIHaveAnExcelFile(excelFile, DEFAULT_FOLDER);
        }

        [Given("I have an Excel file named {string} in the {string} folder")]
        public void GivenIHaveAnExcelFile(string excelFile, string folder)
        {
            if (!File.Exists(folder+"\\"+excelFile))
            {
                throw new Exception(" File not found: " + excelFile);
            }
        }

        [When("I convert the Excel file to a feature file")]
        public void WhenIConvertTheExcelFileToAFeatureFile()
        {
            //TODO:
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
