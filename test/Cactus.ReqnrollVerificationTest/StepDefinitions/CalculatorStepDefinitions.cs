using NUnit.Framework;

namespace Cactus.ReqnrollSampleTest.StepDefinitions
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        // For additional details on Reqnroll step definitions see https://go.reqnroll.net/doc-stepdef

        int _firstNumber;
        int _secondNumber;
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


        [Given("I have following two list of numbers")]
        public void GivenIHaveFollowingTwoListOfNumbers(Table table)
        {
            _table = table;
        }

        [When("the two numbers are added")]
        public void WhenTheTwoNumbersAreAdded()
        {
            _result = _firstNumber + _secondNumber;
        }

        [When("I subtract the second number from the first number")]
        public void WhenISubtractTheSecondNumberFromTheFirstNumber()
        {
            _result = _firstNumber - _secondNumber;
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


        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(int result)
        {
            Assert.That(_result, Is.EqualTo(result));
        }
    }
}
