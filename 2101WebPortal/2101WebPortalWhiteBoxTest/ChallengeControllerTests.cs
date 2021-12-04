using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.Net.Http.Headers;
using Vraze.Controllers;
using Vraze.Models;
using Xunit;

namespace _2101WebPortalWhiteBoxTest
{
    public class ChallengeControllerTests
    {
        [Fact]
        public void Index_ReturnRedirectToActionResult_WhenRoleCookieIsFacilitator()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext();

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = controller.Index() as RedirectToActionResult;

                // Assert that the user is redirected to the Challenge Controller's Manage Action (Manage Challenges Page)
                Assert.Equal("Manage", result.ActionName);
                Assert.Equal("Challenge", result.ControllerName);
            }
        }

        [Fact]
        public void Index_ReturnRedirectToActionResult_WhenRoleCookieIsStudent()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext();

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Student", "studentId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = controller.Index() as RedirectToActionResult;

                // Assert that the user is redirected to the Home Controller's Index Action (Home Page)
                Assert.Equal("Index", result.ActionName);
                Assert.Equal("Home", result.ControllerName);
            }
        }

        [Fact]
        public async Task Manage_ReturnViewResult_WhenRoleCookieIsFacilitator()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext();

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = (await controller.Manage()) as ViewResult;

                // Assert that the name of the view returned is 'Index' (Manage Challenge Page)
                Assert.Equal("index", result.ViewName);
            }
        }

        [Fact]
        public async Task Manage_ReturnRedirectToActionResult_WhenRoleCookieIsStudent()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext();

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Student", "studentId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = (await controller.Manage()) as RedirectToActionResult;

                // Assert that the user is redirected to the Home Controller's Index Action (Home Page)
                Assert.Equal("Index", result.ActionName);
                Assert.Equal("Home", result.ControllerName);
            }
        }

        [Fact]
        public void GotoCreateChallengePage_ReturnViewResult_WhenRoleCookieIsFacilitator()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext();

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = controller.GotoCreateChallengePage() as ViewResult;

                // Assert that the name of the view returned is 'create' (Create Challenge Page)
                Assert.Equal("create", result.ViewName);
            }
        }

        [Fact]
        public void GotoCreateChallengePage_ReturnRedirectToActionResult_WhenRoleCookieIsStudent()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext();

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Student", "studentId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = controller.GotoCreateChallengePage() as RedirectToActionResult;

                // Assert that the user is redirected to the Home Controller's Index Action (Home Page)
                Assert.Equal("Index", result.ActionName);
                Assert.Equal("Home", result.ControllerName);
            }
        }

        [Fact]
        public void GotoEditChallengePage_ReturnViewResult_WhenRoleCookieIsFacilitatorAndChallengeIdCorrect()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                if (!context.Challenges.Any())
                {
                    context.Challenges.Add(new Challenge
                    {
                        ChallengeId = 1,
                        MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1638074732/Screenshot_2021-11-28_at_12.42.59_w3rm69.png",
                        Solution = "FLFFRFFFFLFF",
                        IsTutorialChallenge = true,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext();

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = controller.GotoEditChallengePage(1) as ViewResult;

                // Assert that the name of the view returned is 'Edit' (Edit Challenge Page)
                Assert.Equal("edit", result.ViewName);
            }
        }

        [Fact]
        public void GotoEditChallengePage_ReturnRedirectToActionResult_WhenRoleCookieIsFacilitatorAndChallengeIdIncorrect()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts and Challenge into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                if (!context.Facilitators.Any())
                {
                    context.Challenges.Add(new Challenge
                    {
                        ChallengeId = 1,
                        MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1638074732/Screenshot_2021-11-28_at_12.42.59_w3rm69.png",
                        Solution = "FLFFRFFFFLFF",
                        IsTutorialChallenge = true,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext();

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = controller.GotoEditChallengePage(4) as RedirectToActionResult;

                // Assert that the user is redirected to the Challenge Controller's Manage Action (Manage Challenges Page)
                Assert.Equal("Manage", result.ActionName);
                Assert.Equal("Challenge", result.ControllerName);
            }
        }

        [Fact]
        public void GotoEditChallengePage_ReturnRedirectToActionResult_WhenRoleCookieIsStudent()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext();

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Student", "studentId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = controller.GotoEditChallengePage(1) as RedirectToActionResult;

                // Assert that the user is redirected to the Home Controller's Index Action (Home Page)
                Assert.Equal("Index", result.ActionName);
                Assert.Equal("Home", result.ControllerName);
            }
        }

        [Fact]
        public async Task Create_ReturnsOkObjectResultWithSuccessMessage_WhenChallengeInfoIsComplete()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Create a mock Json data containing the information of the Challenge input by a Facilitator
                var json = "{'Hints':['No. of Commands needed is 4','The first command is Go Forward','The second command is Right Turn'],'MapImageUrl':'https://res.cloudinary.com/dj6afbyih/image/upload/v1637652773/ict1004/w89refxygucg8kqxs8ea.png','Solution':'FRLS','IsTutorialChallenge':false,'IsDeleted':false}";

                // Assign the mock Json to a mock MemoryStream
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext()
                {
                    Request = { Body = stream, ContentLength = stream.Length }
                };

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext() {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context) {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = await controller.Create();

                // Assert the Challenge & Hint added into the database
                var challenges = context.Challenges.Include(challenge => challenge.Hints);

                var expectedHintsObj = new List<Hint>();
                expectedHintsObj.Add(new Hint
                {
                    ChallengeId = 1,
                    HintInformation = "No. of Commands needed is 4"
                });
                expectedHintsObj.Add(new Hint
                {
                    ChallengeId = 1,
                    HintInformation = "The first command is Go Forward"
                });
                expectedHintsObj.Add(new Hint
                {
                    ChallengeId = 1,
                    HintInformation = "The second command is Right Turn"
                });

                var expectedObj = new Challenge {
                    ChallengeId = 1,
                    MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1637652773/ict1004/w89refxygucg8kqxs8ea.png",
                    Solution = "FRLS",
                    Hints = expectedHintsObj,
                    IsDeleted = false,
                    IsTutorialChallenge = false
                };

                // Assert that the challenge has been added into the database
                Assert.Contains(challenges, challenge => challenge.ChallengeId == expectedObj.ChallengeId);

                // Assert the return values from the test
                var retObj = Assert.IsType<OkObjectResult>(result);

                // Assert if the returned response object statusCode property is an Integer
                var statusCode = Assert.IsType<int>(retObj.Value.GetType().GetProperty("statusCode").GetValue(retObj.Value));

                // Assert if the returned response object message property is an string
                var message = Assert.IsType<string>(retObj.Value.GetType().GetProperty("message").GetValue(retObj.Value));

                // Assert if the value of the statusCode property is correct
                Assert.Equal(200, statusCode);

                // Assert if the value of the message property is correct
                Assert.Equal("Successfully added new challenge.", message);
            }
        }

        [Fact]
        public async Task Create_ReturnsBadRequestObjectResultWithFailureMessage_WhenChallengeInfoIsIncomplete()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Create a mock Json data containing the incomplete fields of the Challenge input by a Facilitator
                var json = "{'Hints':['No. of Commands needed is 4','The first command is Go Forward','The second command is Right Turn'],'IsTutorialChallenge':false,'IsDeleted':false}";

                // Assign the mock Json to a mock MemoryStream
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext()
                {
                    Request = { Body = stream, ContentLength = stream.Length }
                };

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = await controller.Create();

                // Assert the return values from the test
                var retObj = Assert.IsType<BadRequestObjectResult>(result);

                // Assert if the returned response object message property is an string
                var message = Assert.IsType<string>(retObj.Value.GetType().GetProperty("message").GetValue(retObj.Value));

                // Assert if the value of the message property is correct
                Assert.Equal("Unable to add challenge due to blanks in some fields. Please check", message);
            }
        }

        [Fact]
        public async Task Create_ReturnsBadRequestObjectResultWithFailureMessage_WhenChallengeInfoContainWrongDataType()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Create a mock Json data containing the incomplete fields of the Challenge input by a Facilitator
                var json = "{'MapImageUrl': false,'Solution': []}";

                // Assign the mock Json to a mock MemoryStream
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext()
                {
                    Request = { Body = stream, ContentLength = stream.Length }
                };

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = await controller.Create();

                // Assert the return values from the test
                var retObj = Assert.IsType<BadRequestObjectResult>(result);

                // Assert if the returned response object message property is an string
                var message = Assert.IsType<string>(retObj.Value.GetType().GetProperty("message").GetValue(retObj.Value));

                // Assert if the value of the message property is correct
                Assert.Equal("There was an error trying to add this challenge, please contact the system administrator.", message);
            }
        }

        [Fact]
        public async Task Edit_ReturnsOkObjectResultWithSuccessMessage_WhenChallengeInfoIsComplete()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts, Challenge & Hint into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                if (!context.Hints.Any())
                {
                    context.Hints.AddRange(new Hint
                    {
                        HintId = 1,
                        HintInformation = "No. of Commands needed is 4",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 2,
                        HintInformation = "The first command is Go Forward",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 3,
                        HintInformation = "The second command is Right Turn",
                        ChallengeId = 1
                    });
                }

                if (!context.Challenges.Any())
                {
                    context.Challenges.Add(new Challenge
                    {
                        ChallengeId = 1,
                        Solution = "FRLS",
                        MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1637652773/ict1004/w89refxygucg8kqxs8ea.png",
                        IsTutorialChallenge = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Create a mock Json data containing the information of the Challenge input by a Facilitator
                var json = "{'Hints':['No. of Commands needed is 4','The first command is Go Forward','The second command is Right Turn'],'MapImageUrl':'https://res.cloudinary.com/dj6afbyih/image/upload/v1637652773/ict1004/w89refxygucg8kqxs8ea.png','Solution':'RRLFS','IsTutorialChallenge':true,'IsDeleted':false}";

                // Assign the mock Json to a mock MemoryStream
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext()
                {
                    Request = { Body = stream, ContentLength = stream.Length }
                };

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = await controller.Edit(1);

                // Get the challenge with ChallengeID: 1 in the database
                var challenge = await context.Challenges.Include(challenge => challenge.Hints).FirstOrDefaultAsync(challenge => challenge.ChallengeId == 1);

                // Assert the Solution of the updated Challenge
                Assert.Equal("RRLFS", challenge.Solution);

                // Assert the IsTutorialChallenge flag of the updated Challenge
                Assert.Equal(true, challenge.IsTutorialChallenge);

                // Assert the return values from the test
                var retObj = Assert.IsType<OkObjectResult>(result);

                // Assert if the returned response object statusCode property is an Integer
                var statusCode = Assert.IsType<int>(retObj.Value.GetType().GetProperty("statusCode").GetValue(retObj.Value));

                // Assert if the returned response object message property is an string
                var message = Assert.IsType<string>(retObj.Value.GetType().GetProperty("message").GetValue(retObj.Value));

                // Assert if the value of the statusCode property is correct
                Assert.Equal(200, statusCode);

                // Assert if the value of the message property is correct
                Assert.Equal("Successfully updated challenge.", message);
            }
        }

        [Fact]
        public async Task Edit_ReturnsBadRequestObjectResultWithFailureMessage_WhenChallengeIdDoesNotExist()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts, Challenge & Hint into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                if (!context.Hints.Any())
                {
                    context.Hints.AddRange(new Hint
                    {
                        HintId = 1,
                        HintInformation = "No. of Commands needed is 4",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 2,
                        HintInformation = "The first command is Go Forward",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 3,
                        HintInformation = "The second command is Right Turn",
                        ChallengeId = 1
                    });
                }

                if (!context.Challenges.Any())
                {
                    context.Challenges.Add(new Challenge
                    {
                        ChallengeId = 1,
                        Solution = "FRLS",
                        MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1637652773/ict1004/w89refxygucg8kqxs8ea.png",
                        IsTutorialChallenge = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Create a mock Json data containing the information of the Challenge input by a Facilitator
                var json = "{'Hints':['No. of Commands needed is 4','The first command is Go Forward','The second command is Right Turn'],'MapImageUrl':'https://res.cloudinary.com/dj6afbyih/image/upload/v1637652773/ict1004/w89refxygucg8kqxs8ea.png','Solution':'RRLFS','IsTutorialChallenge':true,'IsDeleted':false}";

                // Assign the mock Json to a mock MemoryStream
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext()
                {
                    Request = { Body = stream, ContentLength = stream.Length }
                };

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = await controller.Edit(2);

                // Assert the return values from the test
                var retObj = Assert.IsType<BadRequestObjectResult>(result);

                // Assert if the returned response object message property is an string
                var message = Assert.IsType<string>(retObj.Value.GetType().GetProperty("message").GetValue(retObj.Value));

                // Assert if the value of the message property is correct
                Assert.Equal("Challenge does not exist", message);
            }
        }

        [Fact]
        public async Task Edit_ReturnsBadRequestObjectResultWithFailureMessage_WhenChallengeInfoIsIncomplete()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts, Challenge & Hint into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                if (!context.Hints.Any())
                {
                    context.Hints.AddRange(new Hint
                    {
                        HintId = 1,
                        HintInformation = "No. of Commands needed is 4",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 2,
                        HintInformation = "The first command is Go Forward",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 3,
                        HintInformation = "The second command is Right Turn",
                        ChallengeId = 1
                    });
                }

                if (!context.Challenges.Any())
                {
                    context.Challenges.Add(new Challenge
                    {
                        ChallengeId = 1,
                        Solution = "FRLS",
                        MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1637652773/ict1004/w89refxygucg8kqxs8ea.png",
                        IsTutorialChallenge = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Create a mock Json data containing the information of the Challenge input by a Facilitator
                var json = "{'Hints':['No. of Commands needed is 4','The first command is Go Forward','The second command is Right Turn'],'Solution':'RRLFS','IsTutorialChallenge':true,'IsDeleted':false}";

                // Assign the mock Json to a mock MemoryStream
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext()
                {
                    Request = { Body = stream, ContentLength = stream.Length }
                };

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = await controller.Edit(1);

                // Assert the return values from the test
                var retObj = Assert.IsType<BadRequestObjectResult>(result);

                // Assert if the returned response object message property is an string
                var message = Assert.IsType<string>(retObj.Value.GetType().GetProperty("message").GetValue(retObj.Value));

                // Assert if the value of the message property is correct
                Assert.Equal("Unable to update challenge due to blanks in some fields. Please check", message);
            }
        }

        [Fact]
        public async Task Edit_ReturnsBadRequestObjectResultWithFailureMessage_WhenChallengeDoesNotHaveExistingHints()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts, Challenge & Hint into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                if (!context.Challenges.Any())
                {
                    context.Challenges.Add(new Challenge
                    {
                        ChallengeId = 1,
                        Solution = "FRLS",
                        MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1637652773/ict1004/w89refxygucg8kqxs8ea.png",
                        IsTutorialChallenge = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Create a mock Json data containing the information of the Challenge input by a Facilitator
                var json = "{'Hints':['No. of Commands needed is 4','The first command is Go Forward','The second command is Right Turn'], 'MapImageUrl': 'https://res.cloudinary.com/dj6afbyih/image/upload/v1637652773/ict1004/w89refxygucg8kqxs8ea.png', 'Solution':'RRLFS','IsTutorialChallenge':true,'IsDeleted':false}";

                // Assign the mock Json to a mock MemoryStream
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext()
                {
                    Request = { Body = stream, ContentLength = stream.Length }
                };

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = await controller.Edit(1);

                // Assert the return values from the test
                var retObj = Assert.IsType<BadRequestObjectResult>(result);

                // Assert if the returned response object message property is an string
                var message = Assert.IsType<string>(retObj.Value.GetType().GetProperty("message").GetValue(retObj.Value));

                // Assert if the value of the message property is correct
                Assert.Equal("Unable to update Hints of this challenge as there were no hints found.", message);
            }
        }

        [Fact]
        public async Task Edit_ReturnsBadRequestObjectResultWithFailureMessage_WhenChallengeInfoContainWrongDataType()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts, Challenge & Hint into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                if (!context.Hints.Any())
                {
                    context.Hints.AddRange(new Hint
                    {
                        HintId = 1,
                        HintInformation = "No. of Commands needed is 4",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 2,
                        HintInformation = "The first command is Go Forward",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 3,
                        HintInformation = "The second command is Right Turn",
                        ChallengeId = 1
                    });
                }

                if (!context.Challenges.Any())
                {
                    context.Challenges.Add(new Challenge
                    {
                        ChallengeId = 1,
                        Solution = "FRLS",
                        MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1637652773/ict1004/w89refxygucg8kqxs8ea.png",
                        IsTutorialChallenge = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Create a mock Json data containing the information of the Challenge input by a Facilitator
                var json = "{'Hints':{'hint1' : 123},'Solution':'RRLFS','IsTutorialChallenge':true,'IsDeleted':false}";

                // Assign the mock Json to a mock MemoryStream
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(json));

                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext()
                {
                    Request = { Body = stream, ContentLength = stream.Length }
                };

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = await controller.Edit(1);

                // Assert the return values from the test
                var retObj = Assert.IsType<BadRequestObjectResult>(result);

                // Assert if the returned response object message property is an string
                var message = Assert.IsType<string>(retObj.Value.GetType().GetProperty("message").GetValue(retObj.Value));

                // Assert if the value of the message property is correct
                Assert.Equal("There was an error trying to update this challenge, please contact the system administrator.", message);
            }
        }

        [Fact]
        public async Task Delete_ReturnsOkObjectResultWithSuccessMessage_WhenChallengeIdIsValid()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts, Challenge & Hint into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                if (!context.Hints.Any())
                {
                    context.Hints.AddRange(new Hint
                    {
                        HintId = 1,
                        HintInformation = "No. of Commands needed is 4",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 2,
                        HintInformation = "The first command is Go Forward",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 3,
                        HintInformation = "The second command is Right Turn",
                        ChallengeId = 1
                    });
                }

                if (!context.Challenges.Any())
                {
                    context.Challenges.Add(new Challenge
                    {
                        ChallengeId = 1,
                        Solution = "FRLS",
                        MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1637652773/ict1004/w89refxygucg8kqxs8ea.png",
                        IsTutorialChallenge = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext();

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = await controller.Delete(1);

                // Get Challenge with ChallengeId 1 from database
                var challenge = await context.Challenges.FirstOrDefaultAsync(challenge => challenge.ChallengeId == 1);

                // Assert if the delete flag is set to True
                Assert.Equal(true, challenge.IsDeleted);

                // Assert the return values from the test
                var retObj = Assert.IsType<OkObjectResult>(result);

                // Assert if the returned response object statusCode property is an Integer
                var statusCode = Assert.IsType<int>(retObj.Value.GetType().GetProperty("statusCode").GetValue(retObj.Value));

                // Assert if the returned response object message property is an string
                var message = Assert.IsType<string>(retObj.Value.GetType().GetProperty("message").GetValue(retObj.Value));

                // Assert if the value of the statusCode property is correct
                Assert.Equal(200, statusCode);

                // Assert if the value of the message property is correct
                Assert.Equal("Successfully deleted challenge.", message);
            }
        }

        [Fact]
        public async Task Delete_ReturnsBadRequestObjectResultWithFailureMessage_WhenChallengeIdIsInvalid()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts, Challenge & Hint into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                if (!context.Hints.Any())
                {
                    context.Hints.AddRange(new Hint
                    {
                        HintId = 1,
                        HintInformation = "No. of Commands needed is 4",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 2,
                        HintInformation = "The first command is Go Forward",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 3,
                        HintInformation = "The second command is Right Turn",
                        ChallengeId = 1
                    });
                }

                if (!context.Challenges.Any())
                {
                    context.Challenges.Add(new Challenge
                    {
                        ChallengeId = 1,
                        Solution = "FRLS",
                        MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1637652773/ict1004/w89refxygucg8kqxs8ea.png",
                        IsTutorialChallenge = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext();

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = await controller.Delete(4);

                // Assert the return values from the test
                var retObj = Assert.IsType<BadRequestObjectResult>(result);

                // Assert if the returned response object message property is an string
                var message = Assert.IsType<string>(retObj.Value.GetType().GetProperty("message").GetValue(retObj.Value));

                // Assert if the value of the message property is correct
                Assert.Equal("There was an error trying to delete this challenge, please contact the system administrator.", message);
            }
        }

        [Fact]
        public async Task Restore_ReturnsOkObjectResultWithSuccessMessage_WhenChallengeIdIsValid()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts, Challenge & Hint into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                if (!context.Hints.Any())
                {
                    context.Hints.AddRange(new Hint
                    {
                        HintId = 1,
                        HintInformation = "No. of Commands needed is 4",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 2,
                        HintInformation = "The first command is Go Forward",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 3,
                        HintInformation = "The second command is Right Turn",
                        ChallengeId = 1
                    });
                }

                if (!context.Challenges.Any())
                {
                    context.Challenges.Add(new Challenge
                    {
                        ChallengeId = 1,
                        Solution = "FRLS",
                        MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1637652773/ict1004/w89refxygucg8kqxs8ea.png",
                        IsTutorialChallenge = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext();

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = await controller.Restore(1);

                // Get Challenge with ChallengeId 1 from database
                var challenge = await context.Challenges.FirstOrDefaultAsync(challenge => challenge.ChallengeId == 1);

                // Assert if the delete flag is set to True
                Assert.Equal(false, challenge.IsDeleted);

                // Assert the return values from the test
                var retObj = Assert.IsType<OkObjectResult>(result);

                // Assert if the returned response object statusCode property is an Integer
                var statusCode = Assert.IsType<int>(retObj.Value.GetType().GetProperty("statusCode").GetValue(retObj.Value));

                // Assert if the returned response object message property is an string
                var message = Assert.IsType<string>(retObj.Value.GetType().GetProperty("message").GetValue(retObj.Value));

                // Assert if the value of the statusCode property is correct
                Assert.Equal(200, statusCode);

                // Assert if the value of the message property is correct
                Assert.Equal("Successfully restored challenge.", message);
            }
        }

        [Fact]
        public async Task Restore_ReturnsBadRequestObjectResultWithFailureMessage_WhenChallengeIdIsInvalid()
        {
            // Configure the in-memory database for the unit test
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "VrazeTestDatabase")
                .Options;

            // Seed Facilitator Accounts, Challenge & Hint into the database
            using (var context = new ApplicationDbContext(options))
            {
                if (!context.Facilitators.Any())
                {
                    context.Facilitators.Add(new Facilitator
                    {
                        FacilitatorId = 1,
                        Username = "instructor1",
                        PasswordHash = "$2a$12$KayIXLK1VnQu641jW7olkuyV1vAErts6vkWsS47xLvXy3IEHQ84K",
                        IsSystemAdmin = false,
                        IsDeleted = false
                    });
                }

                if (!context.Hints.Any())
                {
                    context.Hints.AddRange(new Hint
                    {
                        HintId = 1,
                        HintInformation = "No. of Commands needed is 4",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 2,
                        HintInformation = "The first command is Go Forward",
                        ChallengeId = 1
                    }, new Hint
                    {
                        HintId = 3,
                        HintInformation = "The second command is Right Turn",
                        ChallengeId = 1
                    });
                }

                if (!context.Challenges.Any())
                {
                    context.Challenges.Add(new Challenge
                    {
                        ChallengeId = 1,
                        Solution = "FRLS",
                        MapImageUrl = "https://res.cloudinary.com/dj6afbyih/image/upload/v1637652773/ict1004/w89refxygucg8kqxs8ea.png",
                        IsTutorialChallenge = false,
                        IsDeleted = false
                    });
                }

                context.SaveChanges();
            }

            // Use clean instance of database with seeded data to conduct the unit test
            using (var context = new ApplicationDbContext(options))
            {
                // Creates the Http Context to mock the Http Request being sent
                var httpContext = new DefaultHttpContext();

                // Creates mock cookies data within the Http Request
                var cookie = new StringValues(new string[] { "role=Facilitator", "facilitatorId=1" });
                httpContext.Request.Headers.Add(HeaderNames.Cookie, cookie);

                // Assign the mocked Http Context to a mock Controller Context
                var controllerContext = new ControllerContext()
                {
                    HttpContext = httpContext,
                };

                // Assign the mocked Controller Context to a mock Challenge Controller
                var controller = new ChallengeController(context)
                {
                    ControllerContext = controllerContext
                };

                // Call the controller method to start the test
                var result = await controller.Restore(4);

                // Assert the return values from the test
                var retObj = Assert.IsType<BadRequestObjectResult>(result);

                // Assert if the returned response object message property is an string
                var message = Assert.IsType<string>(retObj.Value.GetType().GetProperty("message").GetValue(retObj.Value));

                // Assert if the value of the message property is correct
                Assert.Equal("There was an error trying to restore this challenge, please contact the system administrator.", message);
            }
        }
    }
}
