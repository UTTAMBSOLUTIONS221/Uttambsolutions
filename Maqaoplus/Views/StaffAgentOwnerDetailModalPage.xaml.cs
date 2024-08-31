using Maqaoplus.ViewModels.PropertyHouseAgent;

namespace Maqaoplus.Views;


public partial class StaffAgentOwnerDetailModalPage : ContentPage
{
    public StaffAgentOwnerDetailModalPage(PropertyHouseAgentViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}