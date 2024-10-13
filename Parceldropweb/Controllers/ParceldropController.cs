using DBL;
using DBL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Parceldropweb.Controllers
{
    [Authorize]
    public class ParceldropController : BaseController
    {
        private readonly BL bl;
        private readonly IWebHostEnvironment _env;

        public ParceldropController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
        }
        #region Parcel Collection Centers
        [HttpGet]
        public async Task<IActionResult> Collections()
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
        #endregion


        #region Collection Parcels
        [HttpGet]
        public async Task<IActionResult> Parcels()
        {
            //var data = await bl.Getparcelcollectioncentersdata();
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Addcollectionparcel(int Parcelid)
        {
            //Parcelcollectioncenters model = new Parcelcollectioncenters();
            //if (Collectioncenterid > 0)
            //{
            //    model = await bl.Getparcelcollectioncentersdatabyid(Collectioncenterid);
            //}
            return PartialView();
        }
        #endregion
    }
}
