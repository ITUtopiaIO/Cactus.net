# Cactus.net 
By ITUtopia LLC

Writing complex [BDD (Behavior-driven development)](https://en.wikipedia.org/wiki/Behavior-driven_development) testing cases in Excel with table and formula, and then convert to Gherkin feature file, which can be used by  [Cucumber](https://cucumber.io/), [Reqnroll](https://github.com/reqnroll/Reqnroll), [Specflow](https://github.com/specstoryai/specflow), etc.

# The project is currently in DIP - Dev In Progress.

Update: 2025-02-24
The initial phase of creating the project skeleton with one sample feature conversion is now completed! The next phase would be implementing the generic features needed.

# Wiki and how to
https://github.com/ITUtopiaIO/Cactus.net/wiki

***USAGE:*** <br>
    Cactus [FileOrPath] [OPTIONS] <br>

***ARGUMENTS:*** <br>
    [FileOrPath] <br>

***OPTIONS:*** <br>

| Option            | Default   | Description   |
| :---              | :---      | :---          |
|-h, --help         |           |Prints help information| 
|-v, --version      |           |Prints version information|
|-e, --ext          |feature    |Specify the file extension of the generated feature file (default is .feature)| 
|-i, --incSubDir    |false      |Specify whether to process subdirectory files or not (default is false)| 
|-t, --tgtDir       |           |Specify the target directory or sub directory to save the generated feature files| 
|-c, --cloak        |false      |Enable cloak mode: do not add date/time stamp to the feature header line| 
|-m, --match        |false      |Match with an exist feature file| 
|-x, --matchExt     |feature    |Specify the file extension to match with (default is .feature)|
|-r, --matchDir     |           |Specify the directory to match with|
|-z, --zombie       |false      |Enable zombie mode: continue processing all files even if errors, exceptions, or mismatches occur|

# Code Struction
Root
 - src
 - lib <br>
   Ported third party libraries.
 - test
   - Cactus.ReqnrollConverterTest <br>
     Using Cactus.net to conver Excel testing cases into feature file, and copy to Cactus.ReqrollVerificationTest 
   - Cactus.ReqnrollVerificationTest <br>
     Using Reqnroll to run the feature files generated from Cactus.ReqrollConvertTest, and verify the features are compatible with Reqnroll w/o issue.
 


# Third Party softwares
Following softwares are used in Cactus. We are really grateful for programers who created them! Full list of used software and individual license information can be found at License.3rd Parties.md

* MiniExcel https://github.com/mini-software/MiniExcel
* DiffPlex https://github.com/mmanela/diffplex
* Reqnroll https://github.com/reqnroll
* Spectre.Console and Spectre.Console.Cli https://github.com/spectreconsole
