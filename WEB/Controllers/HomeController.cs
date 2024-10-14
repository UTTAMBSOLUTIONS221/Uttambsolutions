using DBL;
using DBL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WEB.Models;

namespace WEB.Controllers
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
        public async Task<IActionResult> Visitingmodule()
        {
            var modules = await bl.Getsystemmoduledata();
            Visitingmodulemodel visitingmodule = new Visitingmodulemodel()
            {
                user = SessionUserData.Usermodel,
                modules = modules.ToList()
            };
            return View(visitingmodule);
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
            var blogs = await bl.Getsystemallblogdata(0, 10000);
            return View(blogs);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Ecommerce()
        {
            var ecommerce = await bl.Getsystemorganizationshopproductsdata();
            return View(ecommerce);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Shopproductdetail(long Shopproductid)
        {
            var commerceProducts = await bl.Getsystemorganizationshopproductsdatabyid(Shopproductid);
            return View(commerceProducts);
        }
        [HttpGet]
        [AllowAnonymous]
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
