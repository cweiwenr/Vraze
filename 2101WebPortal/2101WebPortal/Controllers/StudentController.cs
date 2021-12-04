using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Vraze.Models;
using Vraze.Models.WebFormDataModels;

namespace Vraze.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var userRole = this.HttpContext.Request.Cookies["role"]; // Get the user's role stored in the cookie within the web browser

            // If the user has not logged in (no cookie found) OR the user role is not a student, redirect them to the Join Session Page
            if (string.IsNullOrEmpty(userRole) || userRole != "Student")
            {
                return RedirectToAction("JoinSession", "Student");
            }
            else // If the student has joined the session, return the dashboard with all the challenges they can play
            {
                ViewData["role"] = userRole.ToString();

                var studentId = int.Parse(HttpContext.Request.Cookies["studentId"].ToString());
                var accessCode = HttpContext.Request.Cookies["accessCode"].ToString().ToUpper();
                var session = _context.GameSessions.FirstOrDefault(session => session.AccessCode == accessCode);
                var challengeIDList = (!string.IsNullOrEmpty(session.ChallengeList)) ? session.ChallengeList.Split(';').Select(int.Parse).ToList() : new List<int>();
                List<Challenge> challengeList = null;

                if (challengeIDList.Count() > 0)
                {
                    // Grab from the database all the challenges that are added by facilitator to the current Game Session
                    challengeList = _context.Challenges.Where(challenge => challengeIDList.Contains(challenge.ChallengeId)).ToList();
                }
                else
                {
                    challengeList = new List<Challenge>(); // Empty Challenge List
                }

                ViewData["challenges"] = challengeList;
                ViewData["challengeHistory"] = _context.ChallengeHistories.Where(history => history.StudentId == studentId);
                return View("index", challengeList);
            }
        }

        [HttpGet]
        [Route("/Student/Join")]
        public IActionResult Join()
        {
            // Clear all cookies when returning back to login page to prevent unauthorised visit to other pages
            Response.Cookies.Delete("role");
            Response.Cookies.Delete("accessCode");
            Response.Cookies.Delete("facilitatorId");
            Response.Cookies.Delete("studentId");
            Response.Cookies.Delete("accessCode");
            ViewData.Remove("role");

            return View("JoinSession");
        }

        [HttpPost]
        [Route("/Student/Join")]
        public IActionResult JoinSession(IFormCollection studentJoinInfo)
        {
            var session = _context.GameSessions.FirstOrDefault(g => g.AccessCode == studentJoinInfo["AccessCode"].ToString().ToUpper());

            if (session == null)
            {
                ViewData["message"] = "The game session you are trying to join does not exist.";
                return View("JoinSession");
            }

            if (session.IsActive)
            {
                var studentList = session.StudentList.Split(';').ToList();

                if (studentList.IndexOf(studentJoinInfo["StudentId"].ToString()) != -1)
                {
                    CookieOptions option = new CookieOptions();

                    option.Expires = DateTime.Now.AddMinutes(180);

                    Response.Cookies.Append("role", "Student", option);
                    Response.Cookies.Append("studentId", studentJoinInfo["StudentId"].ToString(), option);
                    Response.Cookies.Append("accessCode", session.AccessCode, option);

                    ViewData["role"] = "Student";
                    return RedirectToAction("Index", "Student");
                }
                else
                {
                    ViewData["message"] = "You do not have access to join this session, please ask your facilitator to grant you access";
                    return View("JoinSession");
                }
            }
            else
            {
                ViewData["message"] = "The session you are trying to join has not been started by the facilitator.";
                return View("JoinSession");
            }
        }

        [HttpGet]
        public IActionResult Logout()
        {
            // Clear all cookies when returning back to home page to prevent unauthorised visit to other pages
            Response.Cookies.Delete("role");
            Response.Cookies.Delete("accessCode");
            Response.Cookies.Delete("facilitatorId");
            Response.Cookies.Delete("studentId");
            Response.Cookies.Delete("accessCode");
            ViewData.Remove("role");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("/Student/Play/{challengeId}")]
        public IActionResult Play(int challengeId)
        {
            var userRole = this.HttpContext.Request.Cookies["role"]; // Get the user's role stored in the cookie within the web browser

            // Get the challenge with the challengeId from the database
            var challenge = _context.Challenges.FirstOrDefault(challenge => challenge.ChallengeId == challengeId);

            if (challenge == null)
            {
                return RedirectToAction("Index", "Student"); //If the challenge does not exist, redirect user back to the dashboard
            }

            ViewData["role"] = userRole.ToString();

            return View("Play", challenge);
        }

        [HttpPost]
        [Route("/Student/SubmitCarCommand/{challengeId}")]
        public async Task<IActionResult> SubmitSolution(int challengeId)
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    string json = await reader.ReadToEndAsync();

                    // Gets the current student information from the database
                    var studentId = int.Parse(HttpContext.Request.Cookies["studentId"].ToString());
                    var student = _context.Students.FirstOrDefault(stud => stud.StudentId == studentId);

                    // Gets the current game session information fromt the database
                    var accessCode = HttpContext.Request.Cookies["accessCode"].ToString().ToUpper();
                    var session = _context.GameSessions.FirstOrDefault(session => session.AccessCode == accessCode);

                    // Get the challenge with the challengeId from the database
                    var challenge = _context.Challenges.Include(challenge => challenge.Hints).FirstOrDefault(challenge => challenge.ChallengeId == challengeId);

                    // Get the student's challenge history for the challenge he/she is attempting right now
                    var challengeHistory = _context.ChallengeHistories.Where(history => history.ChallengeId == challengeId && history.SessionId == session.SessionId && history.StudentId == student.StudentId);

                    // Convert the json string back to the SendCarCommandDataModel
                    var webFormData = JsonConvert.DeserializeObject<SendCarCommandDataModel>(json);

                    if (challenge == null)
                    {
                        return RedirectToAction("Index", "Student"); //If the challenge does not exist, redirect user back to the dashboard
                    }

                    string challengeSolution = challenge.Solution;
                    bool isCorrectSolution = false;
                    int points = 0;

                    // Points Calculation
                    if (webFormData.CarCommand.ToUpper() == challengeSolution)
                    {
                        points = 10; // Full points for correct answer
                        isCorrectSolution = true;
                    } else if (webFormData.CarCommand.Length == challengeSolution.Length)
                    {
                        points = 1;

                        foreach (char c in webFormData.CarCommand.ToUpper().ToCharArray())
                        {
                            int count = challengeSolution.Count(f => f == c);

                            if (count > 0) // If there is a command in the Student's solution that matches the Correct Solution, we will give them a point if the current points calculated is less than 7. So the max points they can score for not getting the correct answer is 6 out of 10.
                            {
                                points += (points < 7) ? 1 : 0;
                            }
                        }
                    }
                    else // Totally wrong answer
                    {
                        points = 0;
                    }

                    // Prepare the Challenge History Object to be added into the database
                    var newChallengeHistory = new ChallengeHistory();
                    newChallengeHistory.Points = points;
                    newChallengeHistory.SessionId = session.SessionId;
                    newChallengeHistory.ChallengeId = challenge.ChallengeId;
                    newChallengeHistory.StudentId = student.StudentId;
                    newChallengeHistory.Solution = webFormData.CarCommand.ToUpper();

                    // Add the student's challenge history into the database
                    _context.Add(newChallengeHistory);

                    // Update the student's has completed tutorial challenge flag
                    student.HasCompletedTutorial = challenge.IsTutorialChallenge;

                    // Update student completed tutorial flag
                    _context.Students.Update(student);

                    // Save Changes into Database
                    _context.SaveChanges();

                    //If the student's solution match the solution of the challenge provided by the facilitator, send the commands to the car
                    if (isCorrectSolution)
                    {
                        /**
                         * To Add Coversion of Blocky comamnds to simplified commands to the car
                         **/
                        string simplifiedCommands = $"W{newChallengeHistory.ChallengeHistoryId:D2}{challenge.Solution}";

                        // Start of sending car commands to the robot car
                        using (TcpClient socket = new TcpClient("172.20.10.2", 8080))
                        {
                            while (socket.Connected)
                            {
                                using (NetworkStream stream = socket.GetStream())
                                {
                                    byte[] data = System.Text.Encoding.ASCII.GetBytes(simplifiedCommands);
                                    stream.Write(data, 0, data.Length);
                                    stream.Close();
                                }
                            }

                            socket.Close();
                        }
                        // End of sending car commands to the robot car

                        var responseObject = new
                        {
                            Points = points,
                            Status = "Correct",
                            Message = "Congratulation, you manage to solve this challenge! Take a look at how car move according to your solution!",
                            Hints = ""
                        };

                        return Ok(responseObject);
                    }
                    else
                    {
                        var hintsForStudent = new List<string>();
                        var hints = challenge.Hints.ToList();
                        int hintCount = (challengeHistory.Count() > 3) ? 3 : challengeHistory.Count();
                        for (int i = 0; i < hintCount; i++)
                        {
                            hintsForStudent.Add(hints[i].HintInformation);
                        }

                        var responseObject = new
                        {
                            Points = points,
                            Status = "Incorrect",
                            Message = $"Your solution is incorrect. Current Points {points}.",
                            Hints = hintsForStudent
                        };

                        return Ok(responseObject);
                    }
                }
            }
            catch (Exception ex)
            {
                var responseObject = new {
                    Message = "There was an error when trying to send the car commands to the robot car. Please contact the administrators!"
                };
                return BadRequest(responseObject);
            }
        }
    }
}
