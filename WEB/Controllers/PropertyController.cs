using DBL;
using DBL.Entities;
using DBL.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace WEB.Controllers
{

    [Authorize]
    public class PropertyController : BaseController
    {
        private readonly BL bl;
        public PropertyController(IConfiguration config)
        {
            bl = new BL(Util.ShareConnectionString(config));
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = await bl.Getsystempropertyhousedata();
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addproperty(long Propertyid)
        {
            ViewData["Systemcountylists"] = bl.GetListModel(ListModelType.SystemCounty).Result.Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();
            ViewData["Systemsubcountylists"] = bl.GetListModel(ListModelType.SystemSubCounty).Result.Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();
            ViewData["Systemsubcountywardlists"] = bl.GetListModel(ListModelType.SystemSubCountyWard).Result.Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();
            var data = await bl.Getsystempropertyhousedatabyid(Propertyid);
            return PartialView(data);
        }
        public async Task<JsonResult> Addsystempropertyhousedata(Systemproperty model)
        {
            var resp = await bl.Registersystempropertyhousedata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }

        [HttpGet]
        public async Task<IActionResult> Details(long Propertyid, long Ownerid)
        {
            var data = await bl.Getsystempropertyhousedetaildatabypropertyidandownerid(Propertyid, Ownerid);
            return View(data);
        }
        [HttpGet]
        public async Task<IActionResult> Addpropertyhousetenant(long Houseroomid)
        {
            Propertyhouseroomtenant model = new Propertyhouseroomtenant();
            model.Houseroomid = Houseroomid;
            return PartialView(model);
        }
        [HttpGet]
        public async Task<IActionResult> Addpropertyhouseroom(long Houseroomid)
        {
            ViewData["Systempropertyhousesizelists"] = bl.GetListModel(ListModelType.Systempropertyhousesizes).Result.Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();
            Systempropertyhouserooms model = new Systempropertyhouserooms();
            if (Houseroomid > 0)
            {
                model = await bl.Getsystempropertyhouseroomdatabyid(Houseroomid);
            }
            return PartialView(model);
        }
        public async Task<JsonResult> Addpropertyhouseroomdata(Systempropertyhouserooms model)
        {
            var resp = await bl.Registerpropertyhouseroomdata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
        [HttpGet]
        public JsonResult Getsystemsubcountydatabyid(long Id)
        {
            var Resp = bl.GetListModelById(ListModelType.SystemSubCounty, Id).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            return Json(Resp);
        }
        public async Task<JsonResult> Addsystempropertyhouseroomtenantdata(Propertyhouseroomtenant model)
        {
            var resp = await bl.Registersystempropertyhouseroomtenantdata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
        [HttpGet]
        public JsonResult Getsystemsubcountywarddatabyid(long Id)
        {
            var Resp = bl.GetListModelById(ListModelType.SystemSubCountyWard, Id).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            return Json(Resp);
        }
    }
}
