using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vraze.Models;

namespace Vraze.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Route("/Admin/Home")]
        public ActionResult Index()
        {
            var roleCookie = this.HttpContext.Request.Cookies["role"]; //Get the role of the user from the request's cookie

            //If the user has not logged in (no cookie found) or if the user is not privilleged, return them to the Admin's login page.
            if (string.IsNullOrEmpty(roleCookie) || (roleCookie != "Admin" && roleCookie != "Facilitator"))
            {
                return View("Login");
            }
            else //If the user is privilleged, redirect the user to the Admin's dashboard.
            {
                ViewData["role"] = roleCookie.ToString();
                return View("Index");
            }
        }

        [HttpGet]
        [Route("/Admin/Login")]
        public ActionResult Login()
        {
            // Clear all cookies when returning back to login page to prevent unauthorised visit to other pages
            Response.Cookies.Delete("role");
            Response.Cookies.Delete("accessCode");
            Response.Cookies.Delete("facilitatorId");
            Response.Cookies.Delete("studentId");
            Response.Cookies.Delete("accessCode");
            ViewData.Remove("role");

            return View("Login");
        }

        [HttpPost]
        [Route("/Admin/Login")]
        public IActionResult LoginUser([Bind("Username", "PasswordHash")]Facilitator model)
        {
            // Clear all cookies when returning back to login page to prevent unauthorised visit to other pages
            Response.Cookies.Delete("role");
            Response.Cookies.Delete("accessCode");
            Response.Cookies.Delete("facilitatorId");
            Response.Cookies.Delete("studentId");
            Response.Cookies.Delete("accessCode");
            ViewData.Remove("role");

            var user = _context.Facilitators.FirstOrDefault(f => f.Username == model.Username && f.IsDeleted == false);

            if (user == null)
            {
                ViewData["message"] = "You have entered an invalid username/password. Please try again.";
                return View("Login");
            }

            // Verify the user password if it is correct, user will be authenticated and redirected to the fashboard
            if (BCrypt.Net.BCrypt.Verify(model.PasswordHash, user.PasswordHash))
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddMinutes(180);

                // Create the cookies to store the user's information
                Response.Cookies.Append("role", (user.IsSystemAdmin) ? "Admin" : "Facilitator", option);
                Response.Cookies.Append("facilitatorId", user.FacilitatorId.ToString(), option);

                // Store the user's role within the web application
                ViewData["role"] = (user.IsSystemAdmin) ? "Admin" : "Facilitator";

                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewData["message"] = "You have entered an invalid username/password. Please try again.";
                return View("Login");
            }
        }

        [HttpGet]
        [Route("/Admin/Logout")]
        public IActionResult Logout()
        {
            // Clear all cookies when returning back to login page to prevent unauthorised visit to other pages
            Response.Cookies.Delete("role");
            Response.Cookies.Delete("accessCode");
            Response.Cookies.Delete("facilitatorId");
            Response.Cookies.Delete("studentId");
            Response.Cookies.Delete("accessCode");
            ViewData.Remove("role");

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("/Admin/Manage")]
        public ActionResult Manage()
        {
            var roleCookie = this.HttpContext.Request.Cookies["role"]; //Get the role of the user from the request's cookie

            //If the user has not logged in (no cookie found) or if the user is not privilleged, return them to the Admin's login page.
            if (string.IsNullOrEmpty(roleCookie) || roleCookie != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
            else //If the user is privilleged, redirect the user to the Facilitator Management.
            {
                ViewData["role"] = roleCookie.ToString();

                var facilitators = _context.Facilitators.ToList();

                return View("Manage", facilitators);
            }
        }

        [HttpGet]
        [Route("/Admin/Create")]
        public ActionResult Create()
        {
            var roleCookie = this.HttpContext.Request.Cookies["role"]; //Get the role of the user from the request's cookie

            //If the user has not logged in (no cookie found) or if the user is not privilleged, return them to the Admin's login page.
            if (string.IsNullOrEmpty(roleCookie) || roleCookie != "Admin" )
            {
                return RedirectToAction("Login", "Admin");
            }
            else //If the user is privilleged, redirect the user to the Admin's dashboard.
            {
                ViewData["role"] = roleCookie.ToString();
                return View("Create");
            }
        }

        [HttpPost]
        [Route("/Admin/Create")]
        public async Task<IActionResult> CreateFacilitatorAccount()
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    string json = await reader.ReadToEndAsync();

                    // Convert the Json request body into the Facilitator Entity
                    var facilitatorInfo = JsonConvert.DeserializeObject<Facilitator>(json);

                    // Check if the input username exists in the database
                    if (_context.Facilitators.Any(f => f.Username == facilitatorInfo.Username))
                    {
                        var errorResponseObj = new {
                            message = $"There is an existing account with the username: {facilitatorInfo.Username}"
                        };
                        return BadRequest(errorResponseObj);
                    }

                    // Prepare the facilitator object to be added into the database
                    Facilitator newFacilitator = new Facilitator();
                    newFacilitator.Username = facilitatorInfo.Username;
                    newFacilitator.FacilitatorName = facilitatorInfo.FacilitatorName;
                    newFacilitator.PasswordHash = BCrypt.Net.BCrypt.HashPassword(facilitatorInfo.PasswordHash);
                    newFacilitator.IsSystemAdmin = false;
                    newFacilitator.IsDeleted = false;

                    // Add the facilitator into the database
                    _context.Facilitators.Add(newFacilitator);
                    _context.SaveChanges();

                    // Send success message back to the user
                    var responseObj = new {
                        status = 200,
                        message = "Successfully added new facilitator account!"
                    };
                    return Ok(responseObj);
                }
            }
            catch (Exception ex)
            {
                var errorResponseObj = new {
                    message = ex.Message
                };
                return BadRequest(errorResponseObj);
            }
        }

        [HttpGet]
        [Route("/Admin/Edit/{id}")]
        public ActionResult Edit(int id)
        {
            var roleCookie = this.HttpContext.Request.Cookies["role"]; //Get the role of the user from the request's cookie

            //If the user has not logged in (no cookie found) or if the user is not privilleged, return them to the Admin's login page.
            if (string.IsNullOrEmpty(roleCookie) || roleCookie != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
            else //If the user is privilleged, redirect the user to the Admin's dashboard.
            {
                ViewData["role"] = roleCookie.ToString();

                var facilitator = _context.Facilitators.FirstOrDefault(f => f.FacilitatorId == id);

                // If facilitator id does not exist redirect the user back to the facilitator management
                if (facilitator == null)
                {
                    return RedirectToAction("Manage", "Admin");
                }

                return View("Edit", facilitator);
            }
        }

        [HttpPost]
        [Route("/Admin/Edit/{id}")]
        public async Task<IActionResult> UpdateFacilitatorAccount(int id)
        {
            try
            {
                using (StreamReader reader = new StreamReader(Request.Body, Encoding.UTF8))
                {
                    string json = await reader.ReadToEndAsync();

                    // Convert the Json request body into the Facilitator Entity
                    var facilitatorInfo = JsonConvert.DeserializeObject<Facilitator>(json);
                    var currentFacilitator = _context.Facilitators.FirstOrDefault(f => f.FacilitatorId == id);

                    // Check if the input username exists in the database
                    if (_context.Facilitators.Any(f => f.Username == facilitatorInfo.Username) && facilitatorInfo.Username != currentFacilitator.Username)
                    {
                        var errorResponseObj = new
                        {
                            message = $"There is an existing account with the username: {facilitatorInfo.Username}"
                        };
                        return BadRequest(errorResponseObj);
                    }

                    // Update the current account info
                    currentFacilitator.Username = facilitatorInfo.Username;
                    currentFacilitator.PasswordHash = BCrypt.Net.BCrypt.HashPassword(facilitatorInfo.PasswordHash);
                    currentFacilitator.FacilitatorName = facilitatorInfo.FacilitatorName;

                    // Update the facilitator into the database
                    _context.Facilitators.Update(currentFacilitator);
                    _context.SaveChanges();

                    // Send success message back to the user
                    var responseObj = new
                    {
                        status = 200,
                        message = "Successfully updated facilitator account!"
                    };
                    return Ok(responseObj);
                }
            }
            catch (Exception ex)
            {
                var errorResponseObj = new
                {
                    message = ex.Message
                };
                return BadRequest(errorResponseObj);
            }
        }

        [HttpGet]
        [Route("/Admin/Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var facilitator = await _context.Facilitators.FirstOrDefaultAsync(f => f.FacilitatorId == id);

                facilitator.IsDeleted = true;
                _context.Facilitators.Update(facilitator);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                var errorResponseObj = new
                {
                    message = ex.Message
                };

                return BadRequest(errorResponseObj);
            }
        }

        [HttpGet]
        [Route("/Admin/Restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            try
            {
                var facilitator = await _context.Facilitators.FirstOrDefaultAsync(f => f.FacilitatorId == id);

                facilitator.IsDeleted = false;
                _context.Facilitators.Update(facilitator);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                var errorResponseObj = new
                {
                    message = ex.Message
                };

                return BadRequest(errorResponseObj);
            }
        }
    }
}
