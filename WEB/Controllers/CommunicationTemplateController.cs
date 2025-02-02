﻿using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WEB.Controllers
{
    [Authorize]
    public class CommunicationTemplateController : BaseController
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public CommunicationTemplateController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await bl.Getsystemcommunicationtemplatedata();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addcommunicationtemplate(long Templateid)
        {
            Communicationtemplate data = new Communicationtemplate();
            if (Templateid > 0)
            {
                data = await bl.Getsystemcommunicationtemplatedatabyid(Templateid);
            }
            return PartialView(data);
        }
        public async Task<JsonResult> Addcommunicationtemplatedata(Communicationtemplate model)
        {
            var resp = await bl.Registersystemcommunicationtemplatedata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }

    }
}
