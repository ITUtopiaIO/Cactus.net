Feature: ExcelConverter

#TODO: Need randominze the file name to avoid residual files for false positives.


Scenario Outline: Convert Excel to Feature File
	Given I have an Excel file "<ExcelFile>"
	When I convert the Excel file to a feature file
	Then I should have a feature file "<ConvertedFeature>"
	And the feature file should exactly match with "<ExpectedFeature>"


Examples:
	| case | ExcelFile             | ConvertedFeature         | ExpectedFeature      |
	| Simple Excel    | SampleTest.xlsx       | SampleTest.feature       | SampleTest.exp       |
	| Simple Excel with Space     | SampleTestWSpace.xlsx | SampleTestWSpace.feature | SampleTestWSpace.exp |
	| Excel with Data Grid	     | DataGrid.xlsx         | DataGrid.feature         | DataGrid.exp         |
