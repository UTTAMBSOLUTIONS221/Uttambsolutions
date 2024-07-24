using DBL;
using DBL.Models;
using Jobs.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Jobs.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {

        private readonly BL bl;

        public HomeController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var jobsData = await bl.Getsystemallopportunitydata(0, 1000);
            return View(jobsData);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Jobdetails(long JobId)
        {
            var jobData = await bl.Getsystemopportunitydatabyid(JobId);
            return View(jobData);
        }
        [HttpGet]
        public async Task<IActionResult> Easyapplythisjob(long jobid)
        {
            var jobData = await bl.Getsystemopportunitydatabyid(jobid);
            var viewModel = new JobApplicationViewModel
            {
                Job = jobData,
                User = SessionUserData.Usermodel
            };

            return View(viewModel);
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
