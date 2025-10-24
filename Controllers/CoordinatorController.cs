using Contract_Monthly_System.Models;
using Contract_Monthly_System.Models.Services;
using Microsoft.AspNetCore.Mvc;

public class CoordinatorController : Controller
{
    private readonly Claim_Memory store_;
    public CoordinatorController(Claim_Memory store) => store_ = store;

    public IActionResult Review()
    {
        var db = new Database();
        var pendingClaims = db.GetClaimsByStates(new[] { "Pending" });



        return View(pendingClaims); // View expects List<Claim>
    }



    [HttpPost]
    public IActionResult PreApprove(int id)
    {
        Database db = new Database();
        db.UpdateClaimState(id, "ApprovedByProgrammeCoordinator");
        TempData["Message"] = "Claim approved.";
        return RedirectToAction("Review");
    }

    [HttpPost]
    public IActionResult Reject(int id)
    {
        Database db = new Database();
        db.UpdateClaimState(id, "RejectedByProgrammeCoordinator");
        TempData["Message"] = "Claim rejected.";
        return RedirectToAction("Review");
    }


    public IActionResult Coordinatorboard()
    {
        ViewBag.userFullName = TempData["UserFullName"];
        return View(); // This loads Views/Coordinator/Coordinatorboard.cshtml
    }

}
