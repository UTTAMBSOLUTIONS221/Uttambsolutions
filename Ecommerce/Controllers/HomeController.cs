using DBL;
using Ecommerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace Ecommerce.Controllers
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
            var ecommerce = await bl.Getallsystemstoreitemdata();
            return View(ecommerce);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Shopproductdetail(long Shopproductid)
        {
            var commerceProducts = await bl.Getsystemorganizationshopproductsdatabyid(Shopproductid);
            return View(commerceProducts);
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
