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
    public class SettingsController : Controller
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public SettingsController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await bl.Getsystemcommunicationtemplatedata();
            return View(data);
        }

        #region System Permissions
        [HttpGet]
        public async Task<IActionResult> Systempermissionlist()
        {
            var data = await bl.Getsystempermissiondata();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addsystempermissions(long Permissionid)
        {
            ViewData["Systemmoduleslists"] = bl.GetListModel(ListModelType.SystemModules).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            Systempermissions model = new Systempermissions();
            if (Permissionid > 0)
            {
                model = await bl.Getsystempermissiondatabyid(Permissionid);
            }
            return PartialView(model);
        }
        public async Task<JsonResult> Addsystempermissiondata(Systempermissions model)
        {
            var Resp = await bl.Registersystempermissiondata(JsonConvert.SerializeObject(model));
            return Json(Resp);
        }
        #endregion

        #region System Company
        [HttpGet]
        public async Task<IActionResult> Companylist()
        {
            var data = await bl.Getsystemorganizationdata();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addorganization(long Organizationid)
        {
            SystemOrganization organization = new SystemOrganization();
            if (Organizationid > 0)
            {
                organization = await bl.Getsystemorganizationdatabyid(Organizationid);
            }
            return PartialView(organization);
        }
        public async Task<JsonResult> Addsystemorganizationdata(SystemOrganization model)
        {
            var resp = await bl.Registersystemorganizationdata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
        #endregion
    }
}
