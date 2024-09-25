using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WEB.Controllers
{
    [Authorize]
    public class BlogcategoryController : BaseController
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public BlogcategoryController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await bl.Getsystemblogcategorydata(0, 1000);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addsystemblogcategory(int Blogcategoryid)
        {
            Systemblogcategories blogCategory = new Systemblogcategories();
            if (Blogcategoryid > 0)
            {
                blogCategory = await bl.Getsystemblogcategorydatabyid(Blogcategoryid);
            }
            return PartialView(blogCategory);
        }
        public async Task<JsonResult> Addsystemblogcategorydata(Systemblogcategories model)
        {
            var resp = await bl.Registersystemblogcategorydata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
    }
}
