using Maqaoplus.ViewModels.PropertyHouse;
namespace Maqaoplus.Views.PropertyHouse.Modal;
public partial class HousesRoomDetailModalPage : ContentPage
{
    public HousesRoomDetailModalPage(PropertyHouseDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}