using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Contract_Monthly_System.Models;

namespace Contract_Monthly_System.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }


    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult LecturerDashboard()
    {
        return View();
    }
    public IActionResult Login()
    {
        return View();
    }
    public IActionResult CoordinatorDashboard
        ()
    {
        return View();
    }

    public IActionResult AcademicManagerDashboard()
    {
        return View();
    }

   


  




    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
