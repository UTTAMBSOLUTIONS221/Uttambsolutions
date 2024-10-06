using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse.Modal;


public partial class AddSystemPropertyHouseModalPage : ContentPage
{
    public AddSystemPropertyHouseModalPage(PropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}