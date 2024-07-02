using DBL.Entities;
using DBL.Enum;
using DBL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace WEB.Controllers
{
    [Authorize]
    public class BrandController : BaseController
    {
        private readonly BL bl;
        public BrandController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
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
