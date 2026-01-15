using NUnit.Framework;

namespace Cactus.ReqnrollSampleTest.StepDefinitions
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        // For additional details on Reqnroll step definitions see https://go.reqnroll.net/doc-stepdef
        // This class is simply to verify the testing cases, and may not be well designed for real calculator usage.

        int? _firstNumber;
        int? _secondNumber;
        int _result;
        Table _table;

        [Given("the first number is (.*)")]
        public void GivenTheFirstNumberIs(int number)
        {
            //TODO: implement arrange (precondition) logic
            // For storing and retrieving scenario-specific data see https://go.specflow.org/doc-sharingdata
            // To use the multiline text or the table argument of the scenario,
            // additional string/Table parameters can be defined on the step definition
            // method. 

            //throw new PendingStepException();
            _firstNumber = number;
        }

        [Given("the second number is (.*)")]
        public void GivenTheSecondNumberIs(int number)
        {
            //TODO: implement arrange (precondition) logic

            //throw new PendingStepException();
            _secondNumber = number;
        }

        [Given("I have following two numbers")]
        public void GivenIHaveFollowingTwoNumbers(Table table)
        {
            foreach(var row in table.Rows)
            {
                _firstNumber = int.Parse(row["First Number"]);
                _secondNumber = int.Parse(row["Second Number"]);
            }
        }


        [Given("I have following list of numbers")]
        public void GivenIHaveFollowingTwoListOfNumbers(Table table)
        {
            _table = table;
        }

        [When("the two numbers are added")]
        public void WhenTheTwoNumbersAreAdded()
        {
            _result = _firstNumber.Value + _secondNumber.Value;
        }

        [When("I subtract the second number from the first number")]
        public void WhenISubtractTheSecondNumberFromTheFirstNumber()
        {
            _result = _firstNumber.Value - _secondNumber.Value;
        }

        [When("I call the SumProduct function")]
        public void WhenICallTheSumProductFunction()
        {
            _result = 0;
            foreach (var row in _table.Rows)
            {
                _result += int.Parse(row["First Number"]) * int.Parse(row["Second Number"]);
            }   
        }

        [When("I call the SumAverage function")]
        public void WhenICallTheSumAverageFunction()
        {
            _result = 0;
            foreach (var row in _table.Rows)
            {
                int count = 0;
                int sub = 0;

                foreach (var r in row.Values)
                {
                    if (string.IsNullOrEmpty(r))
                        continue;

                    sub += int.Parse(r);
                    count++;
                }

                _result += sub / count;

            }
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(int result)
        {
            Assert.That(_result, Is.EqualTo(result));
        }
    }
}
