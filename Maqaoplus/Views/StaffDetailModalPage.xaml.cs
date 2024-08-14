using Maqaoplus.ViewModels.PropertyHouse;
namespace Maqaoplus.Views;

public partial class StaffDetailModalPage : ContentPage
{
    public StaffDetailModalPage(PropertyHouseDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}