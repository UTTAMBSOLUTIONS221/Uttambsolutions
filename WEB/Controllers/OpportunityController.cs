using DBL;
using DBL.Entities;
using DBL.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WEB.Controllers
{
    [Authorize]
    public class OpportunityController : Controller
    {
        private readonly BL bl;
        public OpportunityController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var data = await bl.Getsystemallopportuntydata(0, 1000);
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Addopportunity(int Opportunityid)
        {
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
    }
}
