using Maqaoplus.ViewModels.PropertyHouseTenants;
namespace Maqaoplus.Views.PropertyHouseTenants;
public partial class PropertyHousesTenantDetailPage : ContentPage
{

    public PropertyHousesTenantDetailPage(PropertyHousesRoomTenantsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}