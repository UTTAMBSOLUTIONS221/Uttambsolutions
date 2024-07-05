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
        public async Task<IActionResult> Addorganization(long Organizationid)
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

        [HttpGet]
        public async Task<IActionResult> Organizationdetail(long Organizationid)
        {
            var data = await bl.Getsystemorganizationdetaildatabyid(Organizationid);
            return View(data);
        }
        [HttpGet]
        public IActionResult Addproducttoshop(long Organizationid, long Productid,decimal Wholesaleprice, decimal Retailprice, string Productname)
        {
            Organizationshopproducts model = new Organizationshopproducts();
            model.Productid = Productid;
            model.Organizationid = Organizationid;
            model.Wholesaleprice = Wholesaleprice;
            model.Retailprice = Retailprice;
            model.Productname = Productname;
            return PartialView(model);
        }
        public async Task<JsonResult> Addorganizationshopproductsdata(Organizationshopproducts model)
        {
            var resp = await bl.Registerorganizationshopproductdata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
    }
}
