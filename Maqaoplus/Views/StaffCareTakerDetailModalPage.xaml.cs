using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views;

public partial class StaffCareTakerDetailModalPage : ContentPage
{
    public StaffCareTakerDetailModalPage(PropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}