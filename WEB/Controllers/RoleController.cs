using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WEB.Controllers
{
    [Authorize]
    public class RoleController : BaseController
    {
        private readonly BL bl;
        public RoleController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await bl.Getsystemroledata(0, 1000);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addrole(int Roleid)
        {
            SystemRole role = new SystemRole();
            if (Roleid > 0)
            {
                role = await bl.Getsystemroledatabyid(Roleid);
            }
            return PartialView(role);
        }
        public async Task<JsonResult> Addsystemroledata(SystemRole model)
        {
            var resp = await bl.Registersystemroledata(model);
            return Json(resp);
        }
    }
}
