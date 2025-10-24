using Microsoft.AspNetCore.Mvc;

namespace Contract_Monthly_System.Controllers
{
    public class ClaimsController : Controller
    {
        public IActionResult SubmitClaims()
        {
            return View();
        }

        public IActionResult TrackClaims()
        {
            return View();
        }

        
    }
}
