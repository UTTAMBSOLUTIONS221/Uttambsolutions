using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views;
public partial class StaffAgentDetailModalPage : ContentPage
{
    public StaffAgentDetailModalPage(PropertyHouseDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}