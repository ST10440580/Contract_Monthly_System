using Contract_Monthly_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Contract_Monthly_System.Controllers
{
    public class CoordinatorController : Controller
    {
        public IActionResult Review()
        {
            List<CordinatorViewer> queue = new List<CordinatorViewer>()
            {

                new() {Lecturer = "Tshepang Ramohapi",Month= "9" ,Total = 600 , State = ClaimState.Submitted},
                new() {Lecturer = "Junior Kasongo",Month= "10" ,Total = 700 , State = ClaimState.Submitted}


            }; 
            return View(queue);

        }
    }
}
