using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WEB.Controllers
{
    [Authorize]
    public class BrandController : BaseController
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public BrandController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var environment = _env.IsDevelopment() ? "Development" : "Production";
            ViewBag.Environment = environment;
            var data = await bl.Getsystemproductbranddata(0, 1000);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addproductbrand(int Brandid)
        {
            Productbrand productbrand = new Productbrand();
            if (Brandid > 0)
            {
                productbrand = await bl.Getsystemproductbranddatabyid(Brandid);
            }
            return PartialView(productbrand);
        }
        public async Task<JsonResult> Addsystemproductbranddata(Productbrand model)
        {
            var resp = await bl.Registersystemproductbranddata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
    }
}
