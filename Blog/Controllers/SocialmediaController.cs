﻿using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Authorize]
    public class SocialmediaController : BaseController
    {
        private readonly BL bl;
        public SocialmediaController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var socialmedia = await bl.Getsysteusersocialmediadata(SessionUserData.Usermodel.Userid);
            return View(socialmedia);
        }

        [HttpGet]
        public async Task<IActionResult> Addsocialmediapage(long Socialsettingid)
        {
            SocialMediaSettings socialMediaSettings = new SocialMediaSettings();
            if (Socialsettingid > 0)
            {
                socialMediaSettings = await bl.Getsysteusersocialmediadatabyid(Socialsettingid);
            }
            return PartialView(socialMediaSettings);
        }

        public async Task<JsonResult> Addsocialmediapagedata(SocialMediaSettings model)
        {
            var resp = await bl.Registersystemsocialmediapagedata(model);
            return Json(resp);
        }
    }
}

