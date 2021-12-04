using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Vraze.Models;
using Vraze.Models.WebFormDataModels;

namespace Vraze.Controllers
{
    public class ChallengeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChallengeController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Routes the user to the dashboard of the Admin/Facilitator based on their roles
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            var roleCookie = this.HttpContext.Request.Cookies["role"];
            if (string.IsNullOrEmpty(roleCookie))
            {
                return RedirectToAction("Index", "Home"); //Redirect user back to Home Page if the user has not logged in
            }
            else if (roleCookie == "Admin" || roleCookie == "Facilitator")
            {
                return RedirectToAction("Manage", "Challenge"); //Redirect Admin/Facilitator to the Challenge Management Page
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        [Route("/Challenge/Manage")]
        public async Task<IActionResult> Manage()
        {
            var roleCookie = this.HttpContext.Request.Cookies["role"];
            if (string.IsNullOrEmpty(roleCookie) || (roleCookie != "Admin" && roleCookie != "Facilitator"))
            {
                return RedirectToAction("Index", "Home"); //Redirect user back to Home Page if the user has not logged in
            }
            else
            {
                ViewData["role"] = roleCookie; //Store the role of the user inside the view data for authenticating the user
                return View("index", await _context.Challenges.Where(c => c.IsDeleted == false).Include(c => c.Hints).ToListAsync()); //Return the Challenge Management View with the list of challenges added into the database
            }
        }

        /// <summary>
        /// This methods return the view for Editing the Challenge details given its Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/Challenge/Edit/{id}")]
        public IActionResult GotoEditChallengePage(int id)
        {
            var roleCookie = this.HttpContext.Request.Cookies["role"];
            if (string.IsNullOrEmpty(roleCookie) || (roleCookie != "Admin" && roleCookie != "Facilitator"))
            {
                return RedirectToAction("Index", "Home"); //Redirect user back to Home Page if the user has not logged in
            }
            else
            {
                ViewData["role"] = roleCookie; //Store the role of the user inside the view data for authenticating the user
                var challenge = _context.Challenges.Where(c => c.ChallengeId == id).Include(c => c.Hints).FirstOrDefault();

                // Challenge Id does not exist
                if (challenge == null)
                {
                    return RedirectToAction("Manage", "Challenge");
                }

                return View("edit", challenge); //Return the Edit Challenge Information View with the given Challenge Id
            }
        }

        [HttpGet]
        [Route("/Challenge/Create")]
        public IActionResult GotoCreateChallengePage()
        {
            var roleCookie = this.HttpContext.Request.Cookies["role"];
            if (string.IsNullOrEmpty(roleCookie) || (roleCookie != "Admin" && roleCookie != "Facilitator"))
            {
                return RedirectToAction("Index", "Home"); //Redirect user back to Home Page if the user has not logged in
            }
            else
            {
                ViewData["role"] = roleCookie; //Store the role of the user inside the view data for authenticating the user
                return View("create"); //Return the Create New Challenge View
            }
        }

        [HttpPost]
        [Route("/Challenge/Create")]
        public async Task<IActionResult> Create()
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    string json = await reader.ReadToEndAsync();

                    var challengeInfo = JsonConvert.DeserializeObject<ChallengeFormDataModel>(json);

                    var challenge = new Challenge
                    {
                        MapImageUrl = challengeInfo.MapImageUrl,
                        Solution = challengeInfo.Solution,
                        IsTutorialChallenge = challengeInfo.IsTutorialChallenge,
                        IsDeleted = false
                    };

                    if (string.IsNullOrEmpty(challengeInfo.MapImageUrl) || string.IsNullOrEmpty(challengeInfo.Solution))
                    {
                        var errorResponseObj = new { message = "Unable to add challenge due to blanks in some fields. Please check" };

                        return BadRequest(errorResponseObj);
                    }

                    await _context.Challenges.AddAsync(challenge);
                    _context.SaveChanges();

                    foreach (var hintVal in challengeInfo.Hints)
                    {
                        var hint = new Hint
                        {
                            ChallengeId = challenge.ChallengeId,
                            HintInformation = hintVal
                        };

                        await _context.Hints.AddAsync(hint);
                    }

                    _context.SaveChanges();
                }
                string customMessage = "Successfully added new challenge.";

                return Ok(new { statusCode = 200, message = customMessage });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "There was an error trying to add this challenge, please contact the system administrator." });
            }
        }

        [HttpPost]
        [Route("/Challenge/Edit/{id}")]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    string json = await reader.ReadToEndAsync();

                    var challengeInfo = JsonConvert.DeserializeObject<ChallengeFormDataModel>(json);

                    var challenge = await _context.Challenges.Where(c => c.ChallengeId == id).FirstOrDefaultAsync();

                    if (challenge == null)
                    {
                        return BadRequest(new { message = "Challenge does not exist" });
                    }

                    if (string.IsNullOrEmpty(challengeInfo.MapImageUrl) || string.IsNullOrEmpty(challengeInfo.Solution))
                    {
                        var errorResponseObj = new { message = "Unable to update challenge due to blanks in some fields. Please check" };

                        return BadRequest(errorResponseObj);
                    }

                    challenge.IsTutorialChallenge = Convert.ToBoolean(challengeInfo.IsTutorialChallenge);
                    challenge.Solution = challengeInfo.Solution;
                    challenge.MapImageUrl = challengeInfo.MapImageUrl;

                    _context.Challenges.Update(challenge);
                    _context.SaveChanges();

                    var hints = await _context.Hints.Where(h => h.ChallengeId == challenge.ChallengeId).ToListAsync();

                    if (hints.Count() <= 0)
                    {
                        return BadRequest(new { message = "Unable to update Hints of this challenge as there were no hints found." });
                    }
                    int i = 0;
                    foreach (var hint in hints)
                    {
                        hint.HintInformation = challengeInfo.Hints[i];

                        _context.Hints.Update(hint);
                        i++;
                    }

                    _context.SaveChanges();
                }
                string customMessage = "Successfully updated challenge.";

                return Ok(new { statusCode = 200, message = customMessage });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "There was an error trying to update this challenge, please contact the system administrator." });
            }
        }

        [HttpGet]
        [Route("/Challenge/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var challenge = await _context.Challenges.FirstOrDefaultAsync(c => c.ChallengeId == id);

                challenge.IsDeleted = true;
                _context.Challenges.Update(challenge);
                _context.SaveChanges();

                string customMessage = "Successfully deleted challenge.";

                return Ok(new { statusCode = 200, message = customMessage });
            }
            catch (Exception ex)
            {
                var errorResponseObj = new {
                    message = "There was an error trying to delete this challenge, please contact the system administrator."
                };

                return BadRequest(errorResponseObj);
            }
        }

        [HttpGet]
        [Route("/Challenge/Restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            try
            {
                var challenge = await _context.Challenges.FirstOrDefaultAsync(c => c.ChallengeId == id);

                challenge.IsDeleted = false;
                _context.Challenges.Update(challenge);
                _context.SaveChanges();

                string customMessage = "Successfully restored challenge.";

                return Ok(new { statusCode = 200, message = customMessage });
            }
            catch (Exception ex)
            {
                var errorResponseObj = new
                {
                    message = "There was an error trying to restore this challenge, please contact the system administrator."
                };

                return BadRequest(errorResponseObj);
            }
        }
    }
}
