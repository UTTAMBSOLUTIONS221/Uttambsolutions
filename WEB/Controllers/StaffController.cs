using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
