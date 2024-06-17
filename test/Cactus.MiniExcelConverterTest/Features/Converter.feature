Feature: MiniExcelConverter

#Todo: Need randominze the file name to avoid residual files for false positives.

Scenario: Convert a simple Excel file to a feature file
	Given I have an Excel file "SampleTest.xlsx"
	When I convert the Excel file to a feature file
	Then I should have a feature file "SampleTest.feature"
	