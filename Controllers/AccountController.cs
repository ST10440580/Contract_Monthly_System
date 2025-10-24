using Contract_Monthly_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Contract_Monthly_System.Controllers
{
    public class AccountController : Controller
    {
        // GET: Register Page
        [HttpGet]
        public IActionResult Register()
        {
            // Optional: create tables on first load
            Database db = new Database();
            db.creates_table();

            return View(); // Show the registration form
        }

        // POST: Register Page
        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
                Database db = new Database();
                db.store_user(user.UserFullName, user.Email, user.Password, user.Role);

                TempData["Message"] = "Registration successful. Please log in.";
                return RedirectToAction("Login");
            }

            return View(user); // Show form again with validation errors
        }



        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(User user)
        {
            Database db = new Database();
            var foundUser = db.get_user(user.UserFullName, user.Password); // Match form fields

            if (foundUser == null)
            {
                ViewBag.Error = "Invalid name or password.";
                return View();
            }

            ViewBag.userFullName = foundUser.UserFullName;

            return foundUser.Role switch
            {
                "Lecturer" => RedirectToAction("SubmitClaim", "Lecturer"),
                "Coordinator" => RedirectToAction("Coordinatorboard", "Coordinator"),
                "AcademicManager" => RedirectToAction("ACManager", "AcademicManager"),
                _ => RedirectToAction("Login")
            };

        }
        }
    }
