using DemoEasyAuthB2CApp.Models;
using DemoEasyAuthB2CApp.Security;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace DemoEasyAuthB2CApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var principal = (Request.HttpContext.User as CustomClaimsPrincipal);
            return View(new InfoViewModel() 
                { 
                    Email= principal?.Email ?? "unknown",
                    ADCD_ID= principal?.ADCD_ID ?? "unknown",
                    LoyaltiyID = principal?.LoyaltiyID ?? "unknown" 
                });
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}