using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WEB.Controllers
{
    [Authorize]
    public class ModuleController : BaseController
    {
        private readonly BL bl;
        public ModuleController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var modules = await bl.Getsystemmoduledata();
            return View(modules);
        }

        [HttpGet]
        public async Task<IActionResult> AddModule(string code,long Moduleid)
        {
            Systemmodule module = new Systemmodule();
            if (Moduleid>0)
            {
                module = await bl.Getsystemmoduledatabyid(Moduleid);
            }
            return PartialView(module);
        }
        public async Task<JsonResult> Addsystemmoduledata(Systemmodule model)
        {
            var resp = await bl.Addsystemmoduledata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }

    }
}
