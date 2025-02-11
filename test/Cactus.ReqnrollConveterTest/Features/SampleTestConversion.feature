Feature: Sample Test Conversion

Convert sample test form excel to cucumber feature file

Scenario: Sample Test Conversion
	Given I have an Excel file named "SampleTest.xlsx"
	When I convert the Excel file to a feature file
	And I compare the converted feature file with "SampleTest.ExpectedFeature"
	Then I should find these two files match