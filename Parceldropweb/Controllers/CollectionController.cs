using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Parceldropweb.Controllers
{
    [Authorize]
    public class CollectionController : BaseController
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public CollectionController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await bl.Getparcelcollectioncentersdata();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addcollectioncenter(int Collectioncenterid)
        {
            Parcelcollectioncenters model = new Parcelcollectioncenters();
            if (Collectioncenterid > 0)
            {
                model = await bl.Getparcelcollectioncentersdatabyid(Collectioncenterid);
            }
            return PartialView(model);
        }
        public async Task<JsonResult> Addparcelcollectioncenterdata(Parcelcollectioncenters model)
        {
            var resp = await bl.Registerparcelcollectioncenterdata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
    }
}
