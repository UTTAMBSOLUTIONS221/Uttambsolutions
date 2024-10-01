using DBL;
using DBL.Entities;
using DBL.Entities.Tokenization;
using DBL.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Maqaoplusweb.Controllers
{
    [Authorize]
    public class StaffController : BaseController
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public StaffController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<SystemStaff> data = new List<SystemStaff>();
            if (SessionUserData.Usermodel.Rolename == "System Admin")
            {
                data = await bl.Getsystemstaffdata(0, 1000);
            }
            else
            {
                data = await bl.Getsystemstaffdatabyparentid(SessionUserData.Usermodel.Userid);
            }
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
            var resp = await bl.Registersystemportalstaffdata(model);
            return Json(resp);
        }

        public async Task<JsonResult> Getsystemstaffdatabyidnumber(int Idnumber)
        {
            var resp = await bl.Getsystemstaffdatabyidnumber(Idnumber);
            return Json(resp);
        }
        public async Task<JsonResult> Verifystaffaccountdatabyid(int id)
        {
            var resp = await bl.Verifystaffaccountdatabyid(id);
            return Json(resp);
        }

        [HttpGet, HttpPost]
        public async Task<IActionResult> Resendstaffpassword(long Tenantstaffid)
        {
            var Resp = await bl.Resendstaffpassword(Tenantstaffid);
            if (Resp.RespStatus == 0)
            {
                Success(Resp.RespMessage, true);
            }
            else if (Resp.RespStatus == 1)
            {
                Warning(Resp.RespMessage, true);
            }
            else
            {
                Danger(Resp.RespMessage, true);
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Shareslist()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Buyshare()
        {
            ViewData["Systemtokenslists"] = bl.GetListModel(ListModelType.Systemtokens).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            return PartialView();
        }
        public async Task<JsonResult> Registersoftwaretokenpurchase(Tokenpurchase model)
        {
            var resp = await bl.Registersoftwaretokenpurchasedata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
    }
}
