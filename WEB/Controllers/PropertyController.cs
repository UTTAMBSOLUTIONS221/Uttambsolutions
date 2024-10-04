using DBL;
using DBL.Entities;
using DBL.Entities.Mpesa;
using DBL.Enum;
using DBL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using WEB.Services;
namespace WEB.Controllers
{

    [Authorize]
    public class PropertyController : BaseController
    {
        private readonly BL bl;
        private readonly Mpesaservices mpesaservices;
        private readonly IWebHostEnvironment _env;
        public PropertyController(IConfiguration config, IWebHostEnvironment env)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
            mpesaservices = new Mpesaservices();
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Privacy()
        {
            return View();
        }
        #region Property Listing
        [HttpGet]
        public async Task<IActionResult> Propertyhouselisting()
        {
            var environment = _env.IsDevelopment() ? "Development" : "Production";
            ViewBag.Environment = environment;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Addhouselisting(long Houselistingid)
        {
            //var data = await bl.Getsystempropertyhousedatabyid(Houselistingid);
            return PartialView();
        }
        public async Task<JsonResult> Registerhouseproperty(Propertyhouselisting model)
        {
            var resp = await bl.Registerhousepropertydata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
        #endregion


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
            ViewData["Systemhousewatertypelists"] = bl.GetListModel(ListModelType.Systemhousewatertype).Result.Select(x => new SelectListItem
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
            ViewData["Systemkitchentypelists"] = bl.GetListModel(ListModelType.Systemkitchentype).Result.Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();
            Systempropertyhouseroomdata model = new Systempropertyhouseroomdata();
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
        public async Task<IActionResult> Addpropertyhouseroompayment(long Houseroomid, long Houseroomtenantid)
        {
            ViewData["Systempaymentmodelists"] = bl.GetListModel(ListModelType.Systemcashpaymentmodetype).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            TenantHouseRoomPayment model = new TenantHouseRoomPayment();
            model.TenanthouserroomId = Houseroomid;
            model.TenantId = Houseroomtenantid;
            return PartialView(model);
        }
        [HttpGet]
        public async Task<IActionResult> Addpropertyhouseroommeter(long Houseroomid)
        {
            var model = await bl.Getsystempropertyhouseroommeterdatabyid(Houseroomid);
            return PartialView(model);
        }
        public async Task<JsonResult> Addpropertyhouseroommeterdata(Systempropertyhouseroommeters model)
        {
            var resp = await bl.Registersystempropertyhouseroommeterdata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }


        [HttpGet]
        public async Task<IActionResult> Confirmhouseroompayment(long Houseroomid, long Houseroomtenantid)
        {
            ViewData["Systempaymentmodelists"] = bl.GetListModel(ListModelType.Systemcashpaymentmodetype).Result.Select(x => new SelectListItem { Text = x.Text, Value = x.Value }).ToList();
            TenantHouseRoomPayment model = new TenantHouseRoomPayment();
            model.TenanthouserroomId = Houseroomid;
            model.TenantId = Houseroomtenantid;
            return PartialView(model);
        }
        public async Task<IActionResult> Confirmhouseroompaymentdata(TenantHouseRoomPayment model)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            string securityPassword = "847798hjktfrejtr7438743"; // Your API password or key
            byte[] isoBytes = iso.GetBytes(securityPassword);
            string securityCredential = Convert.ToBase64String(isoBytes);

            var transactionStatusQuery = new TransactionStatusQueryModel
            {
                Initiator = "francis",
                SecurityCredential = securityCredential,
                CommandID = "TransactionStatusQuery",
                TransactionID = "SH81E5K8J5",
                PartyA = "222111",  // Bank Paybill number
                IdentifierType = 4,  // 4 for Paybill, 2 for Till Number
                ResultURL = "https://mainapi.uttambsolutions.com/api/v1/channelm/b2c/result",
                QueueTimeOutURL = "https://mainapi.uttambsolutions.com/api/v1/channelm/b2c/timeout",
                Remarks = "Checking transaction status",
                Occasion = "CustomerPayment"
            };

            var accessToken = await GetAccessTokenAsync();

            using (var client = new System.Net.Http.HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                var jsonContent = JsonConvert.SerializeObject(transactionStatusQuery);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://sandbox.safaricom.co.ke/mpesa/transactionstatus/v1/query", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(result);
                }
                else
                {
                    var errorResponse = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Error Response: " + errorResponse);
                }
            }
            var resp = await bl.Registersystempropertyhouseroommeterdata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
        public async Task<string> GetAccessTokenAsync()
        {
            using (var client = new System.Net.Http.HttpClient())
            {
                var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes("WF5ODAoTZWZlEzkCSIGm6tUzR6RsK2Yk:tAxvOyQdBedKGtiF"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", credentials);

                var response = await client.GetAsync("https://sandbox.safaricom.co.ke/oauth/v1/generate?grant_type=client_credentials");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject(content);
                    return json.access_token;
                }
                else
                {
                    throw new Exception("Unable to get access token");
                }
            }
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
