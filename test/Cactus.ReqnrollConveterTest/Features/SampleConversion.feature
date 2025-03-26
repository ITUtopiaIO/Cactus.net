# This feature file was auto generated from Excel by Cactus.net(https://github.com/ITUtopiaIO/Cactus.net) at 3/26/2025 12:23:03 AM

Feature: SampleConversion

#Sheet: Sample Conversion
    
Scenario Outline: Convert Excel to Feature File and Copy to Reqnoll Verification Folder   
Given I have an Excel file named "<ExcelFile>"  
When I convert the Excel file to a feature file   
Then My converted feature file should match with "<ExpectedFeature>"  
Then I copy the converted feature file to "../../../../Cactus.ReqnrollVerificationTest/Features"  
    
Examples:    
| case                               | ExcelFile                  | ExpectedFeature                       |
| Basic Sample Conversion            | BasicSample.xlsx           | BasicSample.ExpectedFeature           |
| Data Grid Sample Conversion        | DataGridSample.xlsx        | DataGridSample.ExpectedFeature        |
| Scenario Outline Sample Conversion | ScenarioOutlineSample.xlsx | ScenarioOutlineSample.ExpectedFeature |
