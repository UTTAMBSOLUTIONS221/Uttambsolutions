using DBL;
using DBL.Entities;
using DBL.Helpers;
using DBL.Models;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace WEB.Controllers
{
    [Authorize]
    public class OrganizationController : BaseController
    {
        const string SPREADSHEET_ID = "1ahr99NXiAMIgbDxBsnY07Lb2SfRIo81ry4-pSD1tO3U";
        const string SHEET_NAME = "Sokojijicatalog";
        SpreadsheetsResource.ValuesResource _googleSheetValues;
        private readonly BL bl;
        IConfiguration _config;
        public OrganizationController(IConfiguration config, GoogleSheetsHelper googleSheetsHelper)
        {
            bl = new BL(Util.ShareConnectionString(config));
            _config = config;
            _googleSheetValues = googleSheetsHelper.Service.Spreadsheets.Values;
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
        public IActionResult Addproducttoshop(long Organizationid, long Productid, decimal Wholesaleprice, decimal Retailprice, string Productname)
        {
            Organizationshopproducts model = new Organizationshopproducts();
            model.Productid = Productid;
            model.Organizationid = Organizationid;
            model.Wholesaleprice = Wholesaleprice;
            model.Retailprice = Retailprice;
            model.Productname = Productname;
            return PartialView(model);
        }
        [HttpGet]
        public async Task<IActionResult> Editshopproduct(long Shopproductid)
        {
            var data = await bl.Getorganizationshopproductdatabyid(Shopproductid);
            return PartialView(data);
        }
        public async Task<JsonResult> Addorganizationshopproductsdata(Organizationshopproducts model)
        {
            Genericmodel resp = new Genericmodel();
            Organizationshopproductsdata productData = new Organizationshopproductsdata();
            productData = await bl.Registerorganizationshopproductdata(JsonConvert.SerializeObject(model));
            if (productData.Shopproductid > 0)
            {
                try
                {
                    var rangeToCheck = $"{SHEET_NAME}!A1:A";
                    var request = _googleSheetValues.Get(SPREADSHEET_ID, rangeToCheck);
                    var response = request.Execute();
                    var values = response.Values;

                    int rowIndex = -1;
                    if (values != null)
                    {
                        for (int i = 0; i < values.Count; i++)
                        {
                            if (values[i].Count > 0 && values[i][0].ToString() == productData.Productid.ToString())
                            {
                                rowIndex = i + 1; // Adjust for 1-based indexing
                                break;
                            }
                        }
                    }

                    var valueRange = new ValueRange
                    {
                        Values = ItemsMapper.MapToRangeData(new FaceBookItems
                        {
                            Id = productData.Organizationid.ToString(),
                            Title = productData.Productname,
                            Description = productData.Productdescription,
                            Availability = productData.Productavailability,
                            Condition = productData.ProductStatus,
                            Price = productData.Marketprice.ToString("#,##0.00") + " KSH",
                            Link = productData.Producturl,
                            Image_link = productData.Primaryimageurl,
                            Brand = productData.Brandname,
                            Google_product_category = productData.Categoryname,
                            Fb_product_category = productData.Categoryname,
                            Quantity_to_sell_on_facebook = productData.ProductStock.ToString("F2"),
                            Sale_price = productData.Marketprice.ToString("#,##0.00") + " KSH",
                            Sale_price_effective_date = productData.DateCreated.ToString("dd-MM-yyyy"),
                            Item_group_id = productData.Parentcategoryname,
                            Gender = "",
                            Color = productData.ProductColor,
                            Size = productData.ProductSize,
                            Age_group = "",
                            Material = "",
                            Pattern = "",
                            Shipping = "",
                            Shipping_weight = "",
                        })
                    };

                    if (rowIndex != -1)
                    {
                        // If the row exists, update the existing row
                        var updateRequest = _googleSheetValues.Update(valueRange, SPREADSHEET_ID, $"{SHEET_NAME}!A{rowIndex}:I{rowIndex}");
                        updateRequest.ValueInputOption = UpdateRequest.ValueInputOptionEnum.RAW;
                        var updateResponse = updateRequest.Execute();
                    }
                    else
                    {
                        // If the row doesn't exist, append a new row
                        var appendRequest = _googleSheetValues.Append(valueRange, SPREADSHEET_ID, $"{SHEET_NAME}!A{values.Count + 1}:I{values.Count + 1}");
                        appendRequest.ValueInputOption = AppendRequest.ValueInputOptionEnum.USERENTERED;
                        appendRequest.Execute();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error occurred while reading data from Google Sheet: {ex}");
                }
                resp.RespStatus = productData.RespStatus;
                resp.RespMessage = productData.RespMessage;
            }
            else
            {
                resp.RespStatus = 2;
                resp.RespMessage = "Database error occured. Kindly contact system administrator";
            }
            return Json(resp);
        }
    }
}
