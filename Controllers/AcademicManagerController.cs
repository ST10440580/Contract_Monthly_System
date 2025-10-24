using Contract_Monthly_System.Models;
using Contract_Monthly_System.Models.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Contract_Monthly_System.Controllers
{
    public class AcademicManagerController : Controller
    {
        private readonly Claim_Memory store_;
        public AcademicManagerController(Claim_Memory store) => store_ = store;

        public IActionResult ACManager()
        {
            ViewBag.userFullName = TempData["UserFullName"];
            return View();
        }

        public IActionResult ApproveClaims(string month, string lecturer, string state)
        {
            var db = new Database();

            // Only show claims approved by the Programme Coordinator
            var claims = db.GetClaimsByStates(new[] { "ApprovedByProgrammeCoordinator","RejectedByProgrammeCoordinator"});

            if (!string.IsNullOrEmpty(state))
                claims = claims.Where(c => c.State.ToString() == state).ToList();

            if (!string.IsNullOrEmpty(month))
                claims = claims.Where(c => c.Month == month).ToList();

            if (!string.IsNullOrEmpty(lecturer))
                claims = claims.Where(c => c.LecturerName.Contains(lecturer, StringComparison.OrdinalIgnoreCase)).ToList();

            return View(claims);
        }



        [HttpPost]
        public IActionResult Verify(int id)
        {
            Console.WriteLine("Verify action hit with ID: " + id);

            var claim = store_.Get(id);
            if (claim == null) return NotFound();

            claim.State = ClaimState.ApprovedByManager;
            store_.Update(claim);

            TempData["Message"] = "Claim verified by Academic Manager.";
            return RedirectToAction("FinalizedClaims");
        }

     


        [HttpPost]
        public IActionResult Reject(int id)
        {
            var claim = store_.Get(id);
            if (claim == null) return NotFound();

            claim.State = ClaimState.RejectedByManager;
            store_.Update(claim);

            TempData["Message"] = "Claim rejected by Academic Manager.";
            return RedirectToAction("FinalizedClaims");
        }
        public IActionResult FinalizedClaims()
        {
            var db = new Database();
            var finalized = db.GetClaimsByStates(new[] { "ApprovedByManager", "RejectedByManager" });
            return View(finalized);
        }



    }
}
