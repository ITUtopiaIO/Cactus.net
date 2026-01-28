using Cactus.Cucumber;
using NUnit.Framework;

namespace Cactus.ReqnrollTest.StepDefinitions
{
    [Binding]
    public sealed class ConversionStepDefinitions
    {
        const string DEFAULT_FOLDER = "Features";

        string _excelFile = string.Empty;
        string _featureFile = string.Empty;

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
            _featureFile = converter.ConvertExcelToFeature(_excelFile);
        }

        [Then("My converted feature file should match with {string}")]
        public void MyConvertedFeatureFileShouldMatchWith(string expectedFile)
        {
            string _expectedFile = Path.Combine(DEFAULT_FOLDER, expectedFile);
            FileDiff fileDiff = new FileDiff();
            string result = fileDiff.GetFileDiff(_featureFile, _expectedFile, ignoreCommentLine: true);
            Assert.That(result, Is.Empty);

        }

        [Then("I copy the converted feature file to {string}")]
        public void ICopyTheConvertedFeatureFileTo(string folder)
        {
            File.Copy(_featureFile, Path.Combine(folder, Path.GetFileName(_featureFile)), true);
        }

    }
}
