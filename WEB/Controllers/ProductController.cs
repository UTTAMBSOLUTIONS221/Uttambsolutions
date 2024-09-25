using DBL;
using DBL.Entities;
using DBL.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace WEB.Controllers
{
    [Authorize]
    public class ProductController : BaseController
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public ProductController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var environment = _env.IsDevelopment() ? "Development" : "Production";
            ViewBag.Environment = environment;
            var data = await bl.Getsystemproductdata(0, 1000);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addproduct(int Productid)
        {
            ViewData["Systemsubcategorylists"] = bl.GetListModel(ListModelType.SystemCategory).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            ViewData["Systembrandlists"] = bl.GetListModel(ListModelType.Systemproductbrand).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            Systemproducts products = new Systemproducts();
            if (Productid > 0)
            {
                products = await bl.Getsystemproductdatabyid(Productid);
            }
            return PartialView(products);
        }
        public async Task<JsonResult> Addsystemproductdata(Systemproducts model)
        {
            var resp = await bl.Registersystemproductdata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
    }
}
