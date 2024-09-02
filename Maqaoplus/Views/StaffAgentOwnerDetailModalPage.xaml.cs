using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views;


public partial class StaffAgentOwnerDetailModalPage : ContentPage
{
    public StaffAgentOwnerDetailModalPage(PropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}