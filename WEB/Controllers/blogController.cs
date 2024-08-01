using DBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    [Authorize]
    public class blogController : BaseController
    {
        private readonly BL bl;
        public blogController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var blogs = await bl.Getsystemallblogdata(0, 10000);
            return View(blogs);
        }
    }
}
