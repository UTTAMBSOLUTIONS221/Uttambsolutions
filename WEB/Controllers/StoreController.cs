using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
            var data = await bl.Getsystemstoreitemdata();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Addstoreitem(int Storeitemid)
        {
            Systemstoreitems model = new Systemstoreitems();
            if (Storeitemid > 0)
            {
                model = await bl.Getsystemstoreitemdatabyid(Storeitemid);
            }
            return PartialView(model);
        }
        public async Task<JsonResult> Registerstoreproduct(Systemstoreitems model)
        {
            var resp = await bl.Registerstoreproductdata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
    }
}
