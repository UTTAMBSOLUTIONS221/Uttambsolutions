using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse.Modal;

public partial class SystemPropertyHouseRoomCheckListsModalPage : ContentPage
{
    public SystemPropertyHouseRoomCheckListsModalPage(PropertyHouseDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}