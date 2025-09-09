using Contract_Monthly_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Contract_Monthly_System.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(User user)
        {
            if (ModelState.IsValid)
            {
              
                return RedirectToAction("Login");
            }

            return View(user);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User user)
        {

            if (user.Role == "Lecturer")
            {
                TempData["UserFullName"] = user.UserFullName;
                return RedirectToAction("SubmitClaim", "Lecturer");
            }
            else if (user.Role == "Coordinator")
            {
                TempData["UserFullName"] = user.UserFullName;
                return RedirectToAction("Review", "Coordinator");
            }
            else if (user.Role == "AcademicManager")
                TempData["UserFullName"] = user.UserFullName;
            return RedirectToAction("ACManager", "AcademicManager");
        

            ModelState.AddModelError("", "Invalid role or login failed.");
            return View(user);
        }
    }
}
