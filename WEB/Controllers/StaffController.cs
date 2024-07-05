using DBL;
using DBL.Entities;
using DBL.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WEB.Controllers
{
    [Authorize]
    public class StaffController : BaseController
    {
        private readonly BL bl;
        public StaffController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await bl.Getsystemstaffdata(0, 1000);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addstaff(int Userid)
        {
            ViewData["Systemstaffrolelists"] = bl.GetListModel(ListModelType.SystemRoles).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            SystemStaff staffs = new SystemStaff();
            if (Userid > 0)
            {
                staffs = await bl.Getsystemstaffdatabyid(Userid);
            }
            return PartialView(staffs);
        }
        public async Task<JsonResult> Addsystemstaffdata(SystemStaff model)
        {
            var resp = await bl.Registersystemstaffdata(model);
            return Json(resp);
        }
    }
}
