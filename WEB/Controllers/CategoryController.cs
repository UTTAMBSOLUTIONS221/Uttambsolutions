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
    public class CategoryController : BaseController
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public CategoryController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var environment = _env.IsDevelopment() ? "Development" : "Production";
            ViewBag.Environment = environment;
            var data = await bl.Getsystemcategorydata(0, 1000);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addcategory(int Categoryid)
        {
            ViewData["Systemcategorylists"] = bl.GetListModel(ListModelType.SystemCategory).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            Productcategories productcategories = new Productcategories();
            if (Categoryid > 0)
            {
                productcategories = await bl.Getsystemcategorydatabyid(Categoryid);
            }
            return PartialView(productcategories);
        }
        public async Task<JsonResult> Addsystemcategorydata(Productcategories model)
        {
            var resp = await bl.Registersystemcategorydata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
    }
}
