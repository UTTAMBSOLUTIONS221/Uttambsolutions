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

namespace Maqaoplusweb.Controllers
{
    [Authorize]
    public class PropertyHouseController : BaseController
    {
        private readonly BL bl;
        public PropertyHouseController(IConfiguration config)
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
            ViewData["Systemhousewatertypelists"] = bl.GetListModel(ListModelType.Systemhousewatertype).Result.Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();
            List<ListModel> Systemhouserentdueday = new List<ListModel>();
            for (int i = 1; i <= 28; i++)
            {
                string suffix = i switch
                {
                    1 or 21 => "st",
                    2 or 22 => "nd",
                    3 or 23 => "rd",
                    _ => "th"
                };
                Systemhouserentdueday.Add(new ListModel { Value = i.ToString(), Text = $"{i} {suffix} Day" });
            }
            List<ListModel> Systemhousedepostmonths = new List<ListModel>();
            for (int i = 1; i <= 6; i++)
            {
                Systemhousedepostmonths.Add(new ListModel { Value = i.ToString(), Text = $"{i} Month{(i > 1 ? "s" : "")}" });
            }
            List<ListModel> Systemhousevacantnoticeperiod = new List<ListModel>();
            for (int i = 1; i <= 12; i++)
            {
                Systemhousevacantnoticeperiod.Add(new ListModel { Value = i.ToString(), Text = $"{i} Month{(i > 1 ? "s" : "")}" });
            }
            ViewData["Systemhouserentduedaylists"] = Systemhouserentdueday.Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();
            ViewData["Systemhousedepostmonthslists"] = Systemhousedepostmonths.Select(x => new SelectListItem
            {
                Text = x.Text,
                Value = x.Value
            }).ToList();
            ViewData["Systemhousevacantnoticeperiodlists"] = Systemhousevacantnoticeperiod.Select(x => new SelectListItem
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
        public async Task<IActionResult> Propertyhouseroom(long Propertyhouseid)
        {
            // var data = await bl.Getsystempropertyhousedetaildatabyownerid(SessionUserData.Usermodel.Userid);
            var data = await bl.Getsystempropertyhousedetaildatabyhouseid(Propertyhouseid);
            return View(data);
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
            Systempropertyhouserooms modeldata = new Systempropertyhouserooms();
            if (Houseroomid > 0)
            {
                model = await bl.Getsystempropertyhouseroomdatabyid(Houseroomid);
                if (model.Data != null)
                {
                    modeldata = model.Data;
                }
            }
            return PartialView(modeldata);
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
            var resp = await bl.Registerpropertyhouseroommeterdata(JsonConvert.SerializeObject(model));
            return Json(resp);
        }

        [HttpGet]
        public async Task<IActionResult> Propertyhouseroomtenants()
        {
            var data = await bl.Getsystempropertyhouseroomtenantsdata(SessionUserData.Usermodel.Userid);
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Tenantdetails(long Propertytenantid)
        {
            var data = await bl.Getsystempropertyhousetenantdatabytenantid(Propertytenantid);
            return PartialView(data);
        }

        [HttpGet]
        public async Task<IActionResult> Tenantvacatingrequests()
        {
            var data = await bl.Gettenantvacatingrequestsdatabyownerid(SessionUserData.Usermodel.Userid);
            return PartialView(data);
        }
        public async Task<JsonResult> Acceptthisvacatingrequest(SystemPropertyHouseVacatingRequest model)
        {
            var resp = await bl.Approvepropertyhousevacatingrequest(JsonConvert.SerializeObject(model));
            return Json(resp);
        }
        [HttpGet]
        public async Task<IActionResult> Tenantmonthlyinvoicedata()
        {
            var data = await bl.Gettenantmonthlyinvoicedatabyownerid(SessionUserData.Usermodel.Userid);
            return PartialView(data);
        }
        [HttpGet]
        public async Task<IActionResult> TenantmonthlyinvoicePaymentdata()
        {
            var data = await bl.Gettenantmonthlyinvoicepaymentdatabyownerid(SessionUserData.Usermodel.Userid);
            return PartialView(data);
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
            var resp = await bl.Registerpropertyhouseroommeterdata(JsonConvert.SerializeObject(model));
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
