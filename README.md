# Cactus 
By ITUtopia LLC

Writing complex BDD testing cases in Excel with table and formula, and then convert to Cucumber, like Reqnroll, Specflow

# The project is currently in DIP - Dev In Progress.

Update: 2025-02-24
The initial phase of creating the project skeleton with one sample feature conversion is now completed! The next phase would be implementing the generic features needed.

# Wiki and how to
https://github.com/ITUtopiaIO/Cactus.net/wiki

# Code Struction
Root
 - src
 - test
   - Cactus.ReqnrollConverterTest <br>
     Using Cactus.net to conver Excel testing cases into feature file, and copy to Cactus.ReqrollVerificationTest 
   - Cactus.ReqnrollVerificationTest <br>
     Using Reqnroll to run the feature files generated from Cactus.ReqrollConvertTest, and verify the features are compatible with Reqnroll w/o issue.
 


# Third Party softwares
Following softwares are used in Cactus. We are really grateful for programers who created them! Individual license information can be found at License.3rd Parties.md

* DiffPlex: https://github.com/mmanela/diffplex
* MiniExcel: https://github.com/mini-software/MiniExcel
*
