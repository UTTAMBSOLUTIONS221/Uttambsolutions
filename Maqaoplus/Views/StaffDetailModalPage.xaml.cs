using Maqaoplus.ViewModels.PropertyHouse;
namespace Maqaoplus.Views;

public partial class StaffDetailModalPage : ContentPage
{
    public StaffDetailModalPage(PropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}