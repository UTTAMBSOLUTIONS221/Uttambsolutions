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
    public class OpportunityController : BaseController
    {
        private readonly BL bl;
        public OpportunityController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await bl.Getsystemallopportunitydata(0, 1000);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addopportunity(int Opportunityid)
        {
            ViewData["Systemorganizationlists"] = bl.GetListModel(ListModelType.SystemOrganization).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            ViewData["Systemjobfunctionlists"] = bl.GetListModel(ListModelType.Systemjobfunction).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            ViewData["Systemjobindustrylists"] = bl.GetListModel(ListModelType.Systemjobindustry).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            ViewData["Systemjoblocationlists"] = bl.GetListModel(ListModelType.Systemjoblocation).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            ViewData["Systemjobexperiencelists"] = bl.GetListModel(ListModelType.Systemjobexperience).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            ViewData["Systemjobtypelists"] = bl.GetListModel(ListModelType.Systemjobtypeid).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();

            SystemJob systemJob = new SystemJob();
            if (Opportunityid > 0)
            {
                systemJob = await bl.Getsystemopportunitydatabyid(Opportunityid);
            }
            return PartialView(systemJob);
        }
        public async Task<JsonResult> Addsystemopportunitydata(SystemJob model)
        {
            var resp = await bl.Registersystemopportunitydata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }

        public async Task<JsonResult> Publishopportunity(SystemJob model)
        {
            var resp = await bl.Registersystemopportunitydata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }

        [HttpGet]
        public async Task<IActionResult> Opportunitydetail(long Opportunityid)
        {
            var model = new SystemUserLog
            {
                Userid = SessionUserData.Usermodel.Userid,
                Modulename = "Job center",
                Logaction = "Viewing the opportunity details while in the my profile",
                Browser = GetUserBrowser(),
                Ipaddress = Audit.GetIPAddress(),
                Loyaltyreward = 1,
                Loyaltystatus = 1,
                Logactionexittime = 0,
                Datecreated = DateTime.Now,
            };
            bl.Logsystemuseractivitydata(JsonConvert.SerializeObject(model));
            var systemJob = await bl.Getsystemopportunitydatabyid(Opportunityid);
            return PartialView(systemJob);
        }

    }
}
