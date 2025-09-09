using Contract_Monthly_System.Models;


using Microsoft.AspNetCore.Mvc;


namespace Contract_Monthly_System.Controllers
{
    public class LecturerController : Controller
    {
        public IActionResult SubmitClaim()
        {
            LectureViewer modelviewer_ = new LectureViewer
            {
                
                LecturerName = "Tshepang Ramohapi",
               
                HourlyRate = 500.00,
                Claims = new List<ClaimListViewer>

                {
                    new ClaimListViewer {Month = "8" , Module="DATA" ,Hours=8, Total= 600, State=ClaimState.Approved},
                    new ClaimListViewer {Month = "9" , Module="SAND" ,Hours=9, Total= 1200, State=ClaimState.Verified}
                }
            };
            return View(modelviewer_);
        }
    }
}
