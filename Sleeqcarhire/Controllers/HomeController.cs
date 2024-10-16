using DBL;
using DBL.Models.Dashboards;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sleeqcarhire.Models;
using System.Diagnostics;

namespace Sleeqcarhire.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public HomeController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var data = await bl.Getallsystempropertyvacanthouses(0, 10000);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Dashboard()
        {
            PropertyHouseSummaryDashboard model = new PropertyHouseSummaryDashboard();
            if (SessionUserData.Usermodel.Designation == "Owner")
            {
                model = await bl.Getsystempropertyhousedashboardsummarydatabyowner(SessionUserData.Usermodel.Userid);
            }
            else if (SessionUserData.Usermodel.Designation == "Agent")
            {
                model = await bl.Getsystempropertyhousedashboardsummarydatabyagent(SessionUserData.Usermodel.Userid);
            }
            else if (SessionUserData.Usermodel.Designation == "Tenant")
            {
                return RedirectToAction("Tenantprofile", "Home");
            }
            else
            {
                model = await bl.Getsystempropertyhousedashboardsummarydatabyagent(SessionUserData.Usermodel.Userid);
            }
            return View(model);
        }
        public IActionResult Tenantprofile()
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
