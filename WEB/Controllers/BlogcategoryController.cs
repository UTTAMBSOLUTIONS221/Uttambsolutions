using DBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    [Authorize]
    public class BlogcategoryController : BaseController
    {
        private readonly BL bl;
        public BlogcategoryController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await bl.Getsystemblogcategorydata(0, 1000);
            return View(data);
        }
    }
}
