using DBL.Entities;
using Newtonsoft.Json;

namespace Maqaoplus.Views.PropertyHouse
{
    public partial class PropertyHousesDetailPage : ContentPage
    {
        public PropertyHousesDetailPage()
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

            if (parameters.Contains("Property"))
            {
                var queryParams = System.Web.HttpUtility.ParseQueryString(parameters);
                var encodedProperty = queryParams["Property"];
                var jsonProperty = Uri.UnescapeDataString(encodedProperty);
                var property = JsonConvert.DeserializeObject<Systemproperty>(jsonProperty);

                if (property != null)
                {
                    PropertyNameLabel.Text = property.Propertyhousename;
                    PropertyOwnerLabel.Text = property.Propertyhouseownername;
                    PropertyCountyLabel.Text = property.Countyname;
                    PropertySubCountyLabel.Text = property.Subcountyname;
                    PropertyStreetLabel.Text = property.Streetorlandmark;
                    PropertyStatusLabel.Text = property.Propertyhousestatusdata;
                }
            }
        }
    }
}
