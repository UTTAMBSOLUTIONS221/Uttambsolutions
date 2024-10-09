using DBL;
using DBL.Entities;
using DBL.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Authorize]
    public class SocialmediaController : BaseController
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public SocialmediaController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var socialmedia = await bl.Getsystemsocialmediadata(SessionUserData.Usermodel.Userid);
            return View(socialmedia);
        }

        [HttpGet]
        public async Task<IActionResult> Addsocialmediapage(long Socialsettingid)
        {
            SocialMediaSettings socialMediaSettings = new SocialMediaSettings();
            if (Socialsettingid > 0)
            {
                socialMediaSettings = await bl.Getsystemsocialmediadatabyid(Socialsettingid);
            }
            return PartialView(socialMediaSettings);
        }

        //public async Task<JsonResult> Addsocialmediapagedata(SocialMediaSettings model)
        //{
        //    var resp = await bl.Registersystemsocialmediapagedata(model);
        //    return Json(resp);
        //}
    }
}

