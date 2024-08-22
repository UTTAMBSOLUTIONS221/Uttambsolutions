using DBL;
using Maqaoplusweb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Maqaoplusweb.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly BL bl;
        public HomeController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var data = await bl.Getallsystempropertyvacanthouses(0, 10000);
            return View(data);
        }

        public IActionResult Dashboard()
        {
            return View();
        }
        public IActionResult OwnerDashboard()
        {
            return View();
        }
        public IActionResult AgentDashboard()
        {
            return View();
        }
        public IActionResult TenantDashboard()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
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
