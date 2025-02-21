Feature: Sample Test Conversion

Convert sample test form excel to cucumber feature file

Scenario: Sample Test Conversion
	Given I have an Excel file named "SampleTest.xlsx"
	When I convert the Excel file to a feature file
	Then My converted feature file should match with "SampleTest.ExpectedFeature"
	Then I copy the converted feature file to "..\\..\\..\\..\\Cactus.ReqnrollVerificationTest\\Features"
