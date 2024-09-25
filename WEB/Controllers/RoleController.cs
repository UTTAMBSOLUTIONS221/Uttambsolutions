using DBL;
using DBL.Entities;
using DBL.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WEB.Controllers
{
    [Authorize]
    public class RoleController : BaseController
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public RoleController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
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
            ViewData["Systempermissionlists"] = bl.GetListModel(ListModelType.Systempermission).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();

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
