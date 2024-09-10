using Maqaoplus.ViewModels.PropertyHouseTenants;

namespace Maqaoplus.Views.PropertyHouseTenants.Modal;


public partial class AddAgentPropertyHouseTenantPage : ContentPage
{
    public AddAgentPropertyHouseTenantPage(PropertyHousesRoomTenantsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}