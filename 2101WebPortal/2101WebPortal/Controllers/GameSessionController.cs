using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Vraze.Models;
using Vraze.Models.WebFormDataModels;

namespace Vraze.Controllers
{
    public class GameSessionController : Controller
    {
        private readonly ApplicationDbContext _context;

        //Initialises the controller with the database instance
        public GameSessionController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("/GameSession/Manage")]
        public IActionResult Index()
        {
            var userRole = this.HttpContext.Request.Cookies["role"]; // Get the cookie value containing the role of the user from the web browser
            var facilitatorId = int.Parse(this.HttpContext.Request.Cookies["facilitatorId"]); // Get the cookie value containing the facilitator Id

            // If the userRole is null/empty or if the user's role is not an Admin/Facilitator, redirect them back to the home page
            if (string.IsNullOrEmpty(userRole) || (userRole != "Admin" && userRole != "Facilitator"))
            {
                return RedirectToAction("Index", "Home"); //Redirect user back to Home Page if the user has not logged in
            }
            else
            {
                ViewData["role"] = userRole; //Store the role of the user inside the view data for authenticating the user
                return View("index", _context.GameSessions.Where(g => g.CreatedByFacilitatorId == facilitatorId).ToList()); //Return the Game Session Management View with the list of game session added into the database by the facilitator
            }
        }

        [Route("/GameSession/Create")]
        public ActionResult CreateSession()
        {
            var userRole = this.HttpContext.Request.Cookies["role"]; // Get the cookie value containing the role of the user from the web browser

            // If the userRole is null/empty or if the user's role is not an Admin/Facilitator, redirect them back to the home page
            if (string.IsNullOrEmpty(userRole) || (userRole != "Admin" && userRole != "Facilitator"))
            {
                return RedirectToAction("Index", "Home"); //Redirect user back to Home Page if the user has not logged in
            }
            else
            {
                ViewData["role"] = userRole; //Store the role of the user inside the view data for authenticating the user
                return View("CreateSession");
            }
        }

        [HttpGet]
        [Route("/GameSession/Edit/{sessionId}")]
        public ActionResult ModifySession(int sessionId)
        {
            var userRole = this.HttpContext.Request.Cookies["role"]; // Get the cookie value containing the role of the user from the web browser

            // If the userRole is null/empty or if the user's role is not an Admin/Facilitator, redirect them back to the home page
            if (string.IsNullOrEmpty(userRole) || (userRole != "Admin" && userRole != "Facilitator"))
            {
                return RedirectToAction("Index", "Home"); //Redirect user back to Home Page if the user has not logged in
            }
            else
            {
                ViewData["GameSessionId"] = sessionId;
                ViewData["role"] = userRole; //Store the role of the user inside the view data for authenticating the user

                var gameSession = _context.GameSessions.FirstOrDefault(s => s.SessionId == sessionId);

                if (gameSession == null)
                {
                    return RedirectToAction("Index", "GameSession");
                }

                var gameSessionFormData = new GameSessionFormDataModel();

                // Assigning the game session info form the database into the Form Data class to be sent back to the udpate form
                gameSessionFormData.AccessCode = gameSession.AccessCode;
                gameSessionFormData.ChallengeList = gameSession.ChallengeList;
                gameSessionFormData.StudentList = new System.Collections.Generic.List<Student>();

                foreach (var studentId in gameSession.StudentList.Split(';').ToList())
                {
                    var student = _context.Students.FirstOrDefault(s => s.StudentId == int.Parse(studentId));

                    gameSessionFormData.StudentList.Add(student);
                }
                return View("ModifySession", gameSessionFormData);
            }
        }

        [HttpGet]
        [Route("/GameSession/Start/{sessionId}")]
        public ActionResult StartSession(int sessionId)
        {
            var session = _context.GameSessions.FirstOrDefault(session => session.SessionId == sessionId);

            if (session == null)
            {
                return RedirectToAction("Manage", "GameSession");
            }

            session.IsActive = true;
            _context.GameSessions.Update(session);
            _context.SaveChanges();

            return RedirectToAction("Manage", "GameSession");
        }

        [HttpGet]
        [Route("/GameSession/Stop/{sessionId}")]
        public ActionResult StopSession(int sessionId)
        {
            var session = _context.GameSessions.FirstOrDefault(session => session.SessionId == sessionId);

            if (session == null)
            {
                return RedirectToAction("Manage", "GameSession");
            }

            session.IsActive = false;
            _context.GameSessions.Update(session);
            _context.SaveChanges();

            return RedirectToAction("Manage", "GameSession");
        }

        //When the user click 'submit' from the 'CreateSession.cshtml', it will submit a HttpPOST 
        //which will be routed to this method to handle the HttpPost request
        [HttpPost]
        [Route("/GameSession/Create")]
        public async Task<IActionResult> CreateGameSession()
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    string json = await reader.ReadToEndAsync();

                    var gameSessionInfo = JsonConvert.DeserializeObject<GameSessionFormDataModel>(json);

                    var gameSession = _context.GameSessions.FirstOrDefault(session => session.AccessCode == gameSessionInfo.AccessCode.ToUpper());

                    // If there is already a game session with the access code given by the user, do not add this session into the database
                    if (gameSession != null)
                    {
                        var errorResponseObj = new {
                            Status = 404,
                            Message = $"There exist a game session with the access code \"{gameSession.AccessCode.ToUpper()}\". Please give another one."
                        };

                        return Ok(errorResponseObj);
                    }

                    var facilitatorId = HttpContext.Request.Cookies["facilitatorId"]; //Get the facilitator's ID stored in the web browser's cookie

                    // Create a new GameSession object to add into the database
                    var newGameSession = new GameSession();
                    newGameSession.AccessCode = gameSessionInfo.AccessCode;
                    newGameSession.ChallengeList = gameSessionInfo.ChallengeList;
                    newGameSession.StudentList = string.Join(";", gameSessionInfo.StudentList.Select(student => student.StudentId).ToList());
                    newGameSession.SessionStartTime = new DateTime(); // Assigns default value 01/01/0001 00:00:00
                    newGameSession.SessionEndTime = new DateTime(); // Assigns default value 01/01/0001 00:00:00
                    newGameSession.IsActive = false; // Newly created Game Session are not active because it has not been started by the Facilitator
                    newGameSession.CreatedByFacilitatorId = int.Parse(facilitatorId); // Parse the facilitator ID from string to int to store into the database

                    _context.GameSessions.Add(newGameSession); //Add the newly created GameSession into the Database
                    _context.SaveChanges(); // Update the database with the newly added GameSession

                    // Add new students into the Student table
                    foreach (var student in gameSessionInfo.StudentList)
                    {
                        var studObj = _context.Students.FirstOrDefault(s => s.StudentId == student.StudentId);

                        if (studObj == null) // If the student id does not exists in the Student table, add it into the table
                        {
                            student.HasCompletedTutorial = false;
                            _context.Students.Add(student);
                        }
                    }

                    _context.SaveChanges();

                    var responseObj = new {
                        Status = 200,
                        Message = "Successfully added the game session"
                    };

                    return Ok(responseObj); //Once the new GameSession has been successfully added into the database, redirect the user to the Manage Game Session Page (localhost:5000/GameSession/Index)
                }
            }
            catch (Exception ex) //If there is an error, display error message in the create form
            {
                var responseObj = new {
                    Status = 404,
                    Message = "An error has occurred while trying to add the Game Session information, please contact an administrator!"
                }; //Stores the error message inside the server's storage to display in the returned view

                return Ok(responseObj);
            }
        }

        [HttpPost]
        [Route("/GameSession/Update/{sessionId}")]
        public async Task<IActionResult> UpdateGameSession(int sessionId)
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    string json = await reader.ReadToEndAsync();

                    var gameSessionInfo = JsonConvert.DeserializeObject<GameSessionFormDataModel>(json);

                    var updateGameSession = _context.GameSessions.FirstOrDefault(s => s.SessionId == sessionId);
                    var gameSession = _context.GameSessions.FirstOrDefault(session => session.AccessCode == gameSessionInfo.AccessCode.ToUpper());

                    // If there is already a game session with the access code given by the user, do not add this session into the database
                    if (gameSession != null && (gameSession.AccessCode != updateGameSession.AccessCode))
                    {
                        var errorResponseObj = new
                        {
                            Status = 404,
                            Message = $"There exist a game session with the access code \"{gameSession.AccessCode.ToUpper()}\". Please give another one."
                        };

                        return Ok(errorResponseObj);
                    }

                    var facilitatorId = HttpContext.Request.Cookies["facilitatorId"]; //Get the facilitator's ID stored in the web browser's cookie

                    // Update GameSession object to add into the database
                    updateGameSession.AccessCode = gameSessionInfo.AccessCode;
                    updateGameSession.ChallengeList = gameSessionInfo.ChallengeList;
                    updateGameSession.StudentList = string.Join(";", gameSessionInfo.StudentList.Select(student => student.StudentId).ToList());
                    updateGameSession.SessionStartTime = new DateTime(); // Assigns default value 01/01/0001 00:00:00
                    updateGameSession.SessionEndTime = new DateTime(); // Assigns default value 01/01/0001 00:00:00
                    updateGameSession.CreatedByFacilitatorId = int.Parse(facilitatorId); // Parse the facilitator ID from string to int to store into the database

                    _context.GameSessions.Update(updateGameSession); //UpdateGameSession into the Database
                    _context.SaveChanges(); // Save the changes into the database

                    // Add new students into the Student table
                    foreach (var student in gameSessionInfo.StudentList)
                    {
                        var studObj = _context.Students.FirstOrDefault(s => s.StudentId == student.StudentId);

                        if (studObj == null) // If the student id does not exists in the Student table, add it into the table
                        {
                            student.HasCompletedTutorial = false;
                            _context.Students.Add(student);
                        }
                    }

                    _context.SaveChanges();

                    var responseObj = new
                    {
                        Status = 200,
                        Message = "Successfully updated the game session"
                    };

                    return Ok(responseObj); //Once the new GameSession has been successfully added into the database, redirect the user to the Manage Game Session Page (localhost:5000/GameSession/Index)
                }
            }
            catch (Exception ex) //If there is an error, display error message in the create form
            {
                var responseObj = new
                {
                    Status = 404,
                    Message = "An error has occurred while trying to add the Game Session information, please contact an administrator!"
                }; //Stores the error message inside the server's storage to display in the returned view

                return Ok(responseObj);
            }
        }
    }
}