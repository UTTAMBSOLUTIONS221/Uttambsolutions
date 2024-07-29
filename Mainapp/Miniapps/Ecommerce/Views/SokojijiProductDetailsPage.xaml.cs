using DBL.Models;
using Newtonsoft.Json;

namespace Mainapp.Miniapps.Ecommerce.Views
{
    public partial class SokojijiProductDetailsPage : ContentPage
    {
        public SokojijiProductDetailsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            HandleQueryParameters();
        }

        private void HandleQueryParameters()
        {
            // Retrieve the query parameters from the Shell's current navigation state
            var query = Shell.Current.CurrentState.Location.OriginalString;
            var parameters = new Uri(query, UriKind.RelativeOrAbsolute).Query;

            if (parameters.Contains("Product"))
            {
                var queryParams = System.Web.HttpUtility.ParseQueryString(parameters);
                var encodedProduct = queryParams["Product"];
                var jsonProduct = Uri.UnescapeDataString(encodedProduct);
                var product = JsonConvert.DeserializeObject<Organizationshopproductsdata>(jsonProduct);

                if (product != null)
                {
                    ProductName.Text = product.Productname; // Adjust according to your data model
                    ProductPrice.Text = $"${product.Marketprice}"; // Adjust according to your data model
                }
            }
        }
    }
}
