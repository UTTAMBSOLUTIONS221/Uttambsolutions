using DBL.Entities;
using Newtonsoft.Json;
namespace Maqaoplus.Views.PropertyHouse;

public partial class PropertyHousesDetailPage : ContentPage
{
    public Systemproperty Property { get; set; }

    public PropertyHousesDetailPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        HandleQueryParameters();
    }

    private void HandleQueryParameters()
    {
        var query = Shell.Current.CurrentState.Location.OriginalString;
        var parameters = new Uri(query, UriKind.RelativeOrAbsolute).Query;

        if (parameters.Contains("Property"))
        {
            var queryParams = System.Web.HttpUtility.ParseQueryString(parameters);
            var encodedProperty = queryParams["Property"];
            var jsonProperty = Uri.UnescapeDataString(encodedProperty);
            Property = JsonConvert.DeserializeObject<Systemproperty>(jsonProperty);

            if (Property != null)
            {
                // Setting the BindingContext to the deserialized object to bind all properties and lists
                BindingContext = Property;
            }
        }
    }
}