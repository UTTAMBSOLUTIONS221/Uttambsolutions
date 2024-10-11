using DBL;
using DBL.Entities;
using DBL.Models;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WEB.Helpers;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace WEB.Controllers
{
    [Authorize]
    public class StoreController : BaseController
    {
        const string SPREADSHEET_ID = "13lWdMa6sfGVnKSCN5Ssj3UCJLXAiWUVwKrJ6ma6GCPE";
        const string SHEET_NAME = "Sokojijicatalog";
        SpreadsheetsResource.ValuesResource _googleSheetValues;
        private readonly BL bl;
        IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        public StoreController(IConfiguration config, IWebHostEnvironment env, GoogleSheetsHelper googleSheetsHelper)
        {
            bl = new BL(Util.ShareConnectionString(config, env));
            _env = env;
            _config = config;
            _googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var environment = _env.IsDevelopment() ? "Development" : "Production";
            ViewBag.Environment = environment;
            var data = await bl.Getsystemstoreitemdata();
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Addstoreitem(int Storeitemid)
        {
            Systemstoreitems model = new Systemstoreitems();
            if (Storeitemid > 0)
            {
                model = await bl.Getsystemstoreitemdatabyid(Storeitemid);
            }
            return PartialView(model);
        }
        public async Task<JsonResult> Registerstoreproduct(Systemstoreitems model)
        {
            var resp = await bl.Registerstoreproductdata(JsonConvert.SerializeObject(model));
            if (resp.RespStatus == 0)
            {
                try
                {
                    // Ensure initialization
                    EnsureGoogleSheetServiceIsInitialized();
                    EnsureSpreadsheetIdAndSheetNameAreInitialized();

                    var rangeToCheck = $"{SHEET_NAME}!A1:A";
                    var request = _googleSheetValues.Get(SPREADSHEET_ID, rangeToCheck);
                    var response = request.Execute();
                    var values = response.Values;

                    if (values == null || !values.Any())
                    {
                        // Add headers if the sheet is empty
                        AddSheetHeaders();
                        // Reload the values after adding headers
                        response = request.Execute();
                        values = response.Values;
                    }

                    int rowIndex = FindRowIndex(values, Convert.ToInt32(resp.Data1));
                    var valueRange = await CreateValueRange(Convert.ToInt32(resp.Data1));

                    if (rowIndex != -1)
                    {
                        // Update existing row
                        UpdateExistingRow(valueRange, rowIndex);
                    }
                    else
                    {
                        // Append new row
                        AppendNewRow(valueRange, values.Count + 1);
                    }

                    //resp.RespStatus = productData.RespStatus;
                    //resp.RespMessage = productData.RespMessage;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred while interacting with Google Sheets: {ex}");
                    resp.RespStatus = 2;
                    resp.RespMessage = "An error occurred while updating the Google Sheet. Please try again later.";
                }
            }
            return Json(resp);
        }

        private async Task<ValueRange> CreateValueRange(int Storeitemid)
        {
            var productData = await bl.Getsystemstoreitemdatabyid(Storeitemid);
            string startDate = productData.Datecreated.ToString("yyyy-MM-ddTHH:mm:sszzz");
            string endDate = productData.Datecreated.AddMonths(1).ToString("yyyy-MM-ddTHH:mm:sszzz");
            string salePriceEffectiveDate = $"{startDate}/{endDate}";
            return new ValueRange
            {
                Values = ItemsMapper.MapToRangeData(new FaceBookItems
                {
                    Id = productData.Storeitemid.ToString(),
                    Title = productData.Storeitemname,
                    Description = productData.Productdescription,
                    Availability = productData.Productavailability,
                    Condition = productData.Productstatus,
                    Price = productData.Itemsellingprice.ToString("#,##0.00"),
                    Link = "https://uttambsolutions.com/Home/Shopproductdetail?code=" + Guid.NewGuid().ToString() + "&Shopproductid=" + productData.Storeitemid,
                    Image_link = productData.Storeproductimgurl,
                    Brand = productData.Itembrandname,
                    Google_product_category = productData.Parentcategoryname,
                    Fb_product_category = productData.Parentcategoryname,
                    Quantity_to_sell_on_facebook = productData.Productstock.ToString("#,##0"),
                    Sale_price = productData.Itemsellingprice.ToString("#,##0.00"),
                    Sale_price_effective_date = salePriceEffectiveDate,
                    Item_group_id = productData.Parentcategoryname,
                    Gender = productData.Productgender,
                    Color = productData.Productcolor,
                    Size = productData.Itemsize,
                    Age_group = productData.Productagegroup,
                    Material = productData.Productmaterial,
                    Pattern = "",
                    Shipping = "",
                    Shipping_weight = "",
                })
            };
        }

        private int FindRowIndex(IList<IList<object>> values, long shopProductId)
        {
            if (values != null)
            {
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i].Count > 0 && values[i][0].ToString() == shopProductId.ToString())
                    {
                        return i + 1; // Adjust for 1-based indexing
                    }
                }
            }

            return -1;
        }
        private void EnsureGoogleSheetServiceIsInitialized()
        {
            if (_googleSheetValues == null)
            {
                throw new InvalidOperationException("Google Sheets service client (_googleSheetValues) is not initialized.");
            }
        }

        private void EnsureSpreadsheetIdAndSheetNameAreInitialized()
        {
            if (string.IsNullOrEmpty(SPREADSHEET_ID))
            {
                throw new InvalidOperationException("Spreadsheet ID (SPREADSHEET_ID) is not initialized or is empty.");
            }

            if (string.IsNullOrEmpty(SHEET_NAME))
            {
                throw new InvalidOperationException("Sheet name (SHEET_NAME) is not initialized or is empty.");
            }
        }

        private void AddSheetHeaders()
        {
            var headers = new ValueRange
            {
                Values = new List<IList<object>>
        {
            new List<object>
            {
                "id", "title", "description", "availability", "condition", "price", "link", "image_link", "brand",
                "google_product_category", "fb_product_category", "quantity_to_sell_on_facebook", "sale_price",
                "sale_price_effective_date", "item_group_id", "gender", "color", "size", "age_group", "material",
                "pattern", "shipping", "shipping_weight", "video[0].url", "video[0].tag[0]", "style[0]"
            }
        }
            };

            var headerRequest = _googleSheetValues.Update(headers, SPREADSHEET_ID, $"{SHEET_NAME}!A1:Z1");
            headerRequest.ValueInputOption = UpdateRequest.ValueInputOptionEnum.USERENTERED;
            headerRequest.Execute();
        }

        private void UpdateExistingRow(ValueRange valueRange, int rowIndex)
        {
            var updateRequest = _googleSheetValues.Update(valueRange, SPREADSHEET_ID, $"{SHEET_NAME}!A{rowIndex}:Z{rowIndex}");
            updateRequest.ValueInputOption = UpdateRequest.ValueInputOptionEnum.RAW;
            updateRequest.Execute();
        }

        private void AppendNewRow(ValueRange valueRange, int rowIndex)
        {
            var appendRequest = _googleSheetValues.Append(valueRange, SPREADSHEET_ID, $"{SHEET_NAME}!A{rowIndex}:Z{rowIndex}");
            appendRequest.ValueInputOption = AppendRequest.ValueInputOptionEnum.USERENTERED;
            appendRequest.Execute();
        }
    }
}
