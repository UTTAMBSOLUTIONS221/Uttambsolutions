using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse.Modal;

public partial class SystemPropertyHouseRoomImagesModalPage : ContentPage
{
    public SystemPropertyHouseRoomImagesModalPage(PropertyHouseDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}