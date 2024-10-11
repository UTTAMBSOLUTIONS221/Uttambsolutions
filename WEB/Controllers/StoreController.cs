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
            var data = await bl.Getsystemstoreitemdata(0, 1000);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Addstoreitem(int Itemid)
        {
            //Dailychurchevent model = new Dailychurchevent();
            //if (Eventid > 0)
            //{
            //    model = await bl.Getchurcheventdatabyid(Eventid);
            //}
            return PartialView();
        }
        public async Task<JsonResult> Registerstoreproduct(Systemstoreitems model)
        {
            var resp = await bl.Registerstoreproductdata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
    }
}
