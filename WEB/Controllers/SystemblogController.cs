using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Addsystemblog(int Blogid)
        {
            Systemblog blogData = new Systemblog();
            if (Blogid > 0)
            {
                //blogData = await bl.Getsystemblogcategorydatabyid(Blogid);
            }
            return PartialView(blogData);
        }
    }
}
