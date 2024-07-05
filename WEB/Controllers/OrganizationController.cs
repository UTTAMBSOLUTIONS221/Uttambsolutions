using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace WEB.Controllers
{
    [Authorize]
    public class OrganizationController : BaseController
    {
        private readonly BL bl;
        public OrganizationController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        public async Task<IActionResult> Addorganization(int Organizationid)
        {
            SystemOrganization organization = new SystemOrganization();
            if (Organizationid > 0)
            {
                organization = await bl.Getsystemorganizationdatabyid(Organizationid);
            }
            return PartialView(organization);
        }
        public async Task<JsonResult> Addsystemorganizationdata(SystemOrganization model)
        {
            var resp = await bl.Registersystemorganizationdata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
    }
}
