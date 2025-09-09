using Contract_Monthly_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Contract_Monthly_System.Controllers
{
    public class AcademicManagerController : Controller
    {
        public IActionResult ACManager()
        {
            List<AcademicManager> AM = new List<AcademicManager>()
            {

                new() {AcademicManagerName = "Tshepang Ramohapi",ModuleName= "SAND" ,Month = "10" ,Total=600, State = ClaimState.Approved},
                new() {AcademicManagerName = "Junior Kasongo",ModuleName= "PROG" ,Month = "112" ,Total=600, State = ClaimState.Submitted},

            };
            return View(AM);

        }
    }
}
