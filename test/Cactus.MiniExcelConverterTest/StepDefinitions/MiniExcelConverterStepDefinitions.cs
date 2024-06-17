using Cactus.ExcelConverter.MiniExcelConverter;
using NUnit.Framework;

namespace Cactus.MiniExcelConverterTest.StepDefinitions
{
    [Binding]
    public sealed class MiniExcelConverterStepDefinitions
    {
        const string DEFAULT_FOLDER = "Features";

        string _excelFile = string.Empty;
        string _featureFile = string.Empty;

        [Given("I have an Excel file {string}")]
        public void GivenIHaveAnExcelFile(string excelFile)
        {
            GivenIHaveAnExcelFile(excelFile, DEFAULT_FOLDER);
        }

        [Given("I have an Excel file {string} in the {string} folder")]
        public void GivenIHaveAnExcelFile(string excelFile, string folder)
        {
            _excelFile = Path.Combine(folder, excelFile);
            if (!File.Exists(Path.Combine(folder, excelFile)))
            {
                throw new FileNotFoundException(" File not found: " + excelFile);
            }
        }

        [When("I convert the Excel file to a feature file")]
        public void WhenIConvertTheExcelFileToAFeatureFile()
        {
            Converter converter = new Converter();
            converter.ConvertExcelToFeature(_excelFile);
        }
        
        [Then("I should have a feature file {string}")]
        public void ThenIShouldHaveAFeatureFile(string featureFile)
        {
            _featureFile = Path.Combine(DEFAULT_FOLDER, featureFile);
            Assert.IsTrue(File.Exists(_featureFile));
        }

    }
}
