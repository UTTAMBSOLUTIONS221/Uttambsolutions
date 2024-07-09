using DBL.Entities;
using DBL.Models;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using static Google.Apis.Sheets.v4.SpreadsheetsResource.ValuesResource;

namespace DBL.Helpers
{
    public class GoogleSheetsHelper
    {
        public SheetsService Service { get; set; }
        const string APPLICATION_NAME = "Uttambsolutions";
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        private readonly string SPREADSHEET_ID = "1ahr99NXiAMIgbDxBsnY07Lb2SfRIo81ry4-pSD1tO3U";
        private readonly string SHEET_NAME = "Sokojijicatalog";

        public GoogleSheetsHelper()
        {
            InitializeService();
        }

        private void InitializeService()
        {
            var credential = GetCredentialsFromFile();
            Service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = APPLICATION_NAME
            });
        }

        private GoogleCredential GetCredentialsFromFile()
        {
            GoogleCredential credential;
            using (var stream = new FileStream("client_secrets.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(Scopes);
            }

            return credential;
        }

        public void UpdateOrAppendRow(Organizationshopproductsdata productData)
        {
            try
            {
                var rangeToCheck = $"{SHEET_NAME}!A1:A";
                var request = Service.Spreadsheets.Values.Get(SPREADSHEET_ID, rangeToCheck);
                var response = request.Execute();
                var values = response.Values;

                int rowIndex = -1;
                if (values != null)
                {
                    for (int i = 0; i < values.Count; i++)
                    {
                        if (values[i].Count > 0 && values[i][0].ToString() == productData.Organizationid.ToString())
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
                    var updateRequest = Service.Spreadsheets.Values.Update(valueRange, SPREADSHEET_ID, $"{SHEET_NAME}!A{rowIndex}:I{rowIndex}");
                    updateRequest.ValueInputOption = UpdateRequest.ValueInputOptionEnum.RAW;
                    var updateResponse = updateRequest.Execute();
                }
                else
                {
                    // If the row doesn't exist, append a new row
                    var appendRequest = Service.Spreadsheets.Values.Append(valueRange, SPREADSHEET_ID, $"{SHEET_NAME}!A{values.Count + 1}:I{values.Count + 1}");
                    appendRequest.ValueInputOption = AppendRequest.ValueInputOptionEnum.USERENTERED;
                    appendRequest.Execute();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while updating data in Google Sheet: {ex}");
            }
        }
    }
}
