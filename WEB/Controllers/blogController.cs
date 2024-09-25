using DBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WEB.Controllers
{
    [Authorize]
    public class blogController : BaseController
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public blogController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var blogs = await bl.Getsystemallblogdata(0, 10000);
            return View(blogs);
        }
    }
}
