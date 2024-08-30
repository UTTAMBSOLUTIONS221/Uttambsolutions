using Maqaoplus.ViewModels.PropertyHouseAgent;

namespace Maqaoplus.Views;
public partial class StaffAgentDetailModalPage : ContentPage
{
    public StaffAgentDetailModalPage(PropertyHouseAgentDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}