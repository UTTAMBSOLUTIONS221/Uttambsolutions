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
    public class SystemblogController : Controller
    {
        private readonly BL bl;
        public SystemblogController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var blogData = await bl.Getsystemallblogdata(0, 1000);
            return View(blogData);
        }
        [HttpGet]
        public async Task<IActionResult> Addsystemblog(int Blogid)
        {
            ViewData["Systemblogcategorylists"] = bl.GetListModel(ListModelType.SystemBlogCategory).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            Systemblog blogData = new Systemblog();
            if (Blogid > 0)
            {
                //blogData = await bl.Getsystemblogcategorydatabyid(Blogid);
            }
            return PartialView(blogData);
        }
        public async Task<JsonResult> Addsystemblogdata(Systemblog model)
        {
            var resp = await bl.Registersystemblogdata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
    }
}
