# Pre-requistes
### Visual Studio IDE
* The project is coded using the Asp.NET Core Framework and hence require that an Visual Studio IDE either [Visual Studio Community 2019](https://visualstudio.microsoft.com/downloads/) or [Visual Studio Community 2022](https://visualstudio.microsoft.com/downloads/) be used to build/run the project. 
* Do also ensure that with you have selected the '**ASP.NET and Web Development**' workload when installing the IDE.  

### Additional SDKs
The project is coded using the [.NET Core 3.1.415 SDK](https://dotnet.microsoft.com/download/dotnet/thank-you/sdk-3.1.415-windows-x64-installer) as it has Long Term Support(LTS) and require the [Asp.NET Core 3.1.21 Runtime](https://dotnet.microsoft.com/download/dotnet/thank-you/runtime-aspnetcore-3.1.21-windows-x64-installer) to run the project.

# Building & Running the Project
1. Open the `2101WebPortal.sln` file with a Visual Studio IDE
2. Right-click the Project (`2101WebPortal.csproj`) & Click `Build 2101WebPortal`
3. Once the Build Output Console Window display that there is `0 failed`, Click on the green play icon to Run the project.

# Seed Data
To aid the testing of the system, we have seeded some data into the database for demostration purposes. The information of the seed data can be viewed in the [Wiki](https://github.com/cweiwenr/ICT2101-team-p3-3/wiki/Seed-Data).

# Development Workflow

> + Main branch
>> + Dev branch
>>> Checkout to individual feature branches
>>>> + Work on Feature
>>>> + Commit, push and reference issues
>>>> + Create PR
>>> + Code Review
>>> + Merge PR and handle conflicts
>> + PR to Main for final prototype release

# User Acceptance Testing
The team conducted UAT on the complete system. The State Diagram and Use Case Diagrams have been updated from the M2 report as shown below for the UAT. 

### Use Case Diagram
![WBTestImagePreview](https://res.cloudinary.com/dc6eqgbc0/image/upload/v1638108353/M2_-_use_case_diagram_llqutc.png)

### Full System State Diagram
![WBTestImagePreview](https://res.cloudinary.com/dc6eqgbc0/image/upload/v1638108358/newest_state_-_Copy_of_Copy_of_Page_1_1_rfu02u.png)

### UAT Test Cases
The test cases used for the black box testing can be found in the [Wiki](https://github.com/cweiwenr/ICT2101-team-p3-3/wiki/Test-Cases-for-User-Acceptance-Testing)

### UAT Test Demo
[![WBTestImagePreview](https://res.cloudinary.com/dj6afbyih/image/upload/v1638179472/Screenshot_2021-11-29_at_17.49.17_ed4gwl.png)](https://www.youtube.com/watch?v=kCtZK9qRJxc "ICT2x01 team3-p3 UAT")

# White Box Testing
The team decided to conduct unit testing on the `ChallengeController` class using the built-in testing framework, XUnit offered with Visual Studio IDE. This is because the `ChallengeController` is required to interact with the Database Context class and the Model class to perform create, update and delete operations.

### Unit Test Demo
[![WBTestImagePreview](https://res.cloudinary.com/dj6afbyih/image/upload/v1638017614/ict1004/Screenshot_2021-11-27_at_20.53.09_kdpfxz.png)](https://www.youtube.com/watch?v=JGy0yd1WFb4 "ICT2X01 P3-3 White Box Testing")

### How to Build & Run Unit Test
1. Right-Click the Project `2101WebPortalWhiteBoxTest`
2. Click on `Build 2101WebPortalWhiteBoxTest`
3. Ensure that the Build Output Console Window shows `0 failed`
4. Navigate to the `ChallengeControllerTests.cs` file & Right-Click `Run Tests`

### Unit Test Statistics
The statistics for the unit testing is done mannually as the team could not find a suitable coverage report tool to generate the results. Please refer to the [Unit Testing Test Cases](https://github.com/cweiwenr/ICT2101-team-p3-3#unit-testing-test-cases) section for the coverage report of the Challenge Controller class, `ChallengeController.cs`.

The following is the Control Flow Graph for the Challenge Controller Class:
![CFGIMAGE](https://res.cloudinary.com/dj6afbyih/image/upload/v1638192979/Blank_diagram_-_Page_4_1_bmgtmr.jpg)

### Unit Testing Test Cases
| Test ID | Method Tested           | Condition Tested                                                                                       | Expected Result                                                                                                                         | Coverage Statistic                                           |
| ------- | ----------------------- | ------------------------------------------------------------------------------------------------------ | --------------------------------------------------------------------------------------------------------------------------------------- | ------------------------------------------------------------ |
| WB-1    | Index                   | When the role variable value in the cookies is "Facilitator"                                           | The user should be redirected to the Manage Challenges View                                                                             | Covered Lines: 8<br>Coverable Lines: 16<br>Total Lines: 275  |
| WB-2    | Index                   | When the role variable value in the cookies is "Student"                                               | The user should be redirected to the Home Page                                                                                          | Covered Lines: 8<br>Coverable Lines: 16<br>Total Lines: 275  |
| WB-3    | Manage                  | When the role variable value in the cookies is "Facilitator"                                           | The user should be returned with the Manage Challenges View                                                                             | Covered Lines: 10<br>Coverable Lines: 13<br>Total Lines: 275 |
| WB-4    | Manage                  | When the role variable value in the cookies is "Student"                                               | The user should be redirected to the Home Page                                                                                          | Covered Lines: 8<br>Coverable Lines: 13<br>Total Lines: 275  |
| WB-5    | GotoCreateChallengePage | When the role variable value in the cookies is "Facilitator"                                           | The user should be returned with the Create New Challenge View                                                                          | Covered Lines: 10<br>Coverable Lines: 13<br>Total Lines: 275 |
| WB-6    | GotoCreateChallengePage | When the role variable value in the cookies is "Student"                                               | The user should be redirected to the Home Page                                                                                          | Covered Lines: 8<br>Coverable Lines: 13<br>Total Lines: 275  |
| WB-7    | GotoEditChallengePage   | When the role variable value in the cookies is "Facilitator" and the Challenge ID specify is correct   | The user should be returned the Edit Challenge View                                                                                     | Covered Lines: 14<br>Coverable Lines: 18<br>Total Lines: 275 |
| WB-8    | GotoEditChallengePage   | When the role variable value in the cookies is "Facilitator" and the Challenge ID specify is incorrect | The user should be redirected to the Manage Challenges View                                                                             | Covered Lines: 15<br>Coverable Lines: 18<br>Total Lines: 275 |
| WB-9    | GotoEditChallengePage   | When the role variable value in the cookies is "Student"                                               | The user should be redirected to the Home Page                                                                                          | Covered Lines: 8<br>Coverable Lines: 18<br>Total Lines: 275  |
| WB-10   | Create                  | When the challenge information provided is complete and accurate.                                      | The user will receive a success message "Successfully added new challenge."                                                             | Covered Lines: 23<br>Coverable Lines: 32<br>Total Lines: 275 |
| WB-11   | Create                  | When the challenge information provided is not complete with missing fields.                           | The user will receive a failure message "Unable to add challenge due to blanks in some fields. Please check"                            | Covered Lines: 17<br>Coverable Lines: 32<br>Total Lines: 275 |
| WB-12   | Create                  | When the challenge information provided has fields with wrong data type                                | The user will receive a failure message "There was an error trying to add this challenge, please contact the system adminstrator."      | Covered Lines: 14<br>Coverable Lines: 32<br>Total Lines: 275 |
| WB-13   | Edit                    | When the challenge information provided is complete and accurate.                                      | The user will receive a success message "Successfully updated challenge."                                                               | Covered Lines: 34<br>Coverable Lines: 45<br>Total Lines: 275 |
| WB-14   | Edit                    | When the Challenge Id provided does not exist.                                                         | The user will receive a failure message "Challenge does not exist"                                                                      | Covered Lines: 15<br>Coverable Lines: 45<br>Total Lines: 275 |
| WB-15   | Edit                    | When the challenge information provided is not complete with missing fields.                           | The user will receive a failure message "Unable to update challenge due to blanks in some fields. Please check"                         | Covered Lines: 18<br>Coverable Lines: 45<br>Total Lines: 275 |
| WB-16   | Edit                    | When the challenge being updated does not have any existing hint information.                          | The user will receive a failure message "Unable to update Hints of this challenge as there were no hints found."                        | Covered Lines: 25<br>Coverable Lines: 45<br>Total Lines: 275 |
| WB-17   | Edit                    | When the challenge information provided has fields with wrong data type                                | The user will receive a failure message "There was an error trying to update this challenge, please contact the system adminstrator."   | Covered Lines: 14<br>Coverable Lines: 45<br>Total Lines: 275 |
| WB-18   | Delete                  | When the Challenge ID provided is valid                                                                | The user will receive a success message "Successfully deleted challenge."                                                               | Covered Lines: 12<br>Coverable Lines: 17<br>Total Lines: 275 |
| WB-19   | Delete                  | When the Challenge ID provided does not exist.                                                         | The user will receive a failure message "There was an error trying to delete this challenge, please contact the system administrator."  | Covered Lines: 15<br>Coverable Lines: 17<br>Total Lines: 275 |
| WB-20   | Restore                 | When the Challenge ID provided is valid                                                                | The user will receive the success message "Successfully restored challenge."                                                            | Covered Lines: 12<br>Coverable Lines: 17<br>Total Lines: 275 |
| WB-21   | Restore                 | When the Challenge ID provided does not exist.                                                         | The user will receive a failure message "There was an error trying to restore this challenge, please contact the system administrator." | Covered Lines: 15<br>Coverable Lines: 17<br>Total Lines: 275 |

# Burndown Chart
![BURNDOWNCHART](https://res.cloudinary.com/dc6eqgbc0/image/upload/v1638441811/Picture1_ddh9er.png)

# Gantt Chart
![GANTTCHARTIMAGE](https://res.cloudinary.com/dc6eqgbc0/image/upload/v1638449236/image_2021-12-02_20-45-24_jbloci.png)
