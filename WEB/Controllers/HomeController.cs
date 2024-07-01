using DBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewsAPI;
using NewsAPI.Constants;
using NewsAPI.Models;
using System.Diagnostics;
using WEB.Models;

namespace WEB.Controllers
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
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Moudules()
        {
            var modules = await bl.Getsystemmoduledata();
            return View(modules);
        }
        [HttpGet]
        public IActionResult Dashboard(string? slug)
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Blogs()
        {
            var blogs = await bl.Getsystemblogsdata(0, 10);
            return View(blogs);
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
