using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views;
public partial class StaffAgentDetailModalPage : ContentPage
{
    public StaffAgentDetailModalPage(PropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}