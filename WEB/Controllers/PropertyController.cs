using DBL;
using DBL.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WEB.Controllers
{

    [Authorize]
    public class PropertyController : BaseController
    {
        private readonly BL bl;
        public PropertyController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Addproperty()
        {
            ViewData["Systemcountylists"] = bl.GetListModel(ListModelType.SystemCounty).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();

            return PartialView();
        }
    }
}
