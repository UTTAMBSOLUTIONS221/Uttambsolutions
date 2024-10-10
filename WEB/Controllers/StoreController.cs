using DBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    [Authorize]
    public class StoreController : BaseController
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public StoreController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var environment = _env.IsDevelopment() ? "Development" : "Production";
            ViewBag.Environment = environment;
            //var data = await bl.Getsystemproductbranddata(0, 1000);
            return View();
        }
    }
}
