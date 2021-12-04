using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vraze.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Vraze.Controllers
{
    public class ChallengeHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChallengeHistoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Redirect to student's dashboard if the url does not include the challenge id
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Student");
        }

        [HttpGet]
        [Route("/ChallengeHistory/View/{challengeId}")]
        public IActionResult View(int challengeId)
        {
            var userRole = this.HttpContext.Request.Cookies["role"]; // Get the user's role stored in the cookie within the web browser
            var accessCode = HttpContext.Request.Cookies["accessCode"].ToString().ToUpper();
            var studentId = int.Parse(HttpContext.Request.Cookies["studentId"].ToString());

            // If the user has not logged in (no cookie found) OR the user role is not a student, redirect them to the Join Session Page
            if (string.IsNullOrEmpty(userRole) || userRole != "Student")
            {
                return RedirectToAction("JoinSession", "Student");
            }
            else
            {
                var session = _context.GameSessions.FirstOrDefault(session => session.AccessCode == accessCode);
                var challengeHistoryList = _context.ChallengeHistories.Include(history => history.Challenge).Include(history => history.Challenge.Hints).Where(history => history.ChallengeId == challengeId && history.SessionId == session.SessionId && history.StudentId == studentId).ToList();

                ViewData["role"] = userRole.ToString();
                return View("View", challengeHistoryList);
            }
        }

        [HttpGet]
        [Route("/Car/Update")]
        public IActionResult Update([FromQuery(Name = "historyId")] int challengeHistoryId, [FromQuery(Name = "Speed")] double speed, [FromQuery(Name = "Distance")] double distance)
        {
            try
            {
                var currentChallengeHistory = _context.ChallengeHistories.FirstOrDefault(history => history.ChallengeHistoryId == challengeHistoryId);

                if (currentChallengeHistory == null)
                {
                    return BadRequest();
                }

                currentChallengeHistory.CarDistanceTravelled = distance;
                currentChallengeHistory.CarSpeed = speed;

                _context.ChallengeHistories.Update(currentChallengeHistory);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
    }
}