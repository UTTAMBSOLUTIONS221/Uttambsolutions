using DBL;
using DBL.Entities;
using DBL.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            ViewData["Systemcountylists"] = bl.GetListModel(ListModelType.SystemCounty).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            ViewData["Systemsubcountylists"] = bl.GetListModel(ListModelType.SystemSubCounty).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            ViewData["Systemsubcountywardlists"] = bl.GetListModel(ListModelType.SystemSubCountyWard).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
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
            var data = await bl.Getcollectioncenterparcelsdata();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addcollectionparcel(int Parcelid)
        {
            ViewData["Collectioncenterlists"] = bl.GetListModel(ListModelType.Collectioncenter).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            ViewData["ParcelSenderRecieverlists"] = bl.GetListModel(ListModelType.ParcelSenderReciever).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            ViewData["Parceltypeslists"] = bl.GetListModel(ListModelType.Parceltypes).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            ViewData["Parcelstatuslists"] = bl.GetListModel(ListModelType.Parcelstatus).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            Collectioncenterparcels model = new Collectioncenterparcels();
            if (Parcelid > 0)
            {
                model = await bl.Getcollectioncenterparcelsdatabyid(Parcelid);
            }
            return PartialView(model);
        }
        public async Task<JsonResult> Addcollectioncenterparceldata(Collectioncenterparcels model)
        {
            var resp = await bl.Registercollectioncenterparceldata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
        #endregion



        [HttpGet]
        public JsonResult Getsystemsubcountydatabyid(long Id)
        {
            var Resp = bl.GetListModelById(ListModelType.SystemSubCounty, Id).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            return Json(Resp);
        }
        [HttpGet]
        public JsonResult Getsystemsubcountywarddatabyid(long Id)
        {
            var Resp = bl.GetListModelById(ListModelType.SystemSubCountyWard, Id).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            return Json(Resp);
        }
    }
}
