using Cactus.CucumberTest;
using Cactus.ExcelConverter;
using NUnit.Framework;
using System.Security.Cryptography.X509Certificates;

namespace Cactus.ExcelConverterTest.StepDefinitions
{
    [Binding]
    public sealed class ExcelConverterStepDefinitions
    {
        readonly string DEFAULT_FOLDER = "Features";

        //string _GUID = string.Empty;
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
            //_GUID = System.Guid.NewGuid().ToString();
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
            ThenIShouldHaveAFeatureFile(featureFile, DEFAULT_FOLDER);
        }

        [Then("I should have a feature file {string} in the {string} folder")]
        public void ThenIShouldHaveAFeatureFile(string featureFile, string folder)
        {
            _featureFile = Path.Combine(folder, featureFile);
            Assert.That(File.Exists(_featureFile), Is.True);
        }

        [Then("the feature file should exactly match with {string}")]
        public void ThenTheFeatureFileExactlyShouldMatchWith(string expectedFile)
        {
            ThenTheFeatureFileShouldExactlyMatchWith(expectedFile, DEFAULT_FOLDER);
        }

        [Then("the feature file should exactly match with {string} in the {string} folder")]
        public void ThenTheFeatureFileShouldExactlyMatchWith(string expectedFile, string folder)
        {
            CheckFeatureFileMatchWith(expectedFile, folder, ignoreTableFormat: false);
        }

        [Then("the feature file should match with {string}")]
        public void ThenTheFeatureFileShouldMatchWith(string expectedFile)
        {
            ThenTheFeatureFileShouldMatchWith(expectedFile, DEFAULT_FOLDER);
        }

        [Then("the feature file should match with {string} in the {string} folder")]
        public void ThenTheFeatureFileShouldMatchWith(string expectedFile, string folder)
        {
            CheckFeatureFileMatchWith(expectedFile, folder, ignoreTableFormat: true);
        }

        private void CheckFeatureFileMatchWith(string expectedFile, string folder, bool ignoreTableFormat)
        {
            FileDiff fileDiff = new FileDiff();
            string _expectedFile = Path.Combine(folder, expectedFile);
            string result = fileDiff.GetFileDiff(_featureFile, _expectedFile, ignoreEmptyLine: false, ignoreTableFormat);
            Assert.That(result, Is.Empty);
        }



        //private string RandomizeFileName(string fileName)
        //{
        //    return Path.Combine(Path.GetDirectoryName(fileName), Path.GetFileNameWithoutExtension(fileName) + _GUID + Path.GetExtension(fileName));
        //}




    }
}
