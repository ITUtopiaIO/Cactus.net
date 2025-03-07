Feature: Sample Conversion Test

Convert sample test form excel to cucumber feature file

Scenario: Basic Sample Conversion
	Given I have an Excel file named "BasicSample.xlsx"
	When I convert the Excel file to a feature file
	Then My converted feature file should match with "BasicSample.ExpectedFeature"
	Then I copy the converted feature file to "../../../../Cactus.ReqnrollVerificationTest/Features"


Scenario: Data Grid Sample Conversion
	Given I have an Excel file named "DataGridSample.xlsx"
	When I convert the Excel file to a feature file
	Then My converted feature file should match with "DataGridSample.ExpectedFeature"
	# Then I copy the converted feature file to "../../../../Cactus.ReqnrollVerificationTest/Features"