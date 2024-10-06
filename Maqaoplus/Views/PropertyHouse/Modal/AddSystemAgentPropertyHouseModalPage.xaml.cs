using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse.Modal;
public partial class AddSystemAgentPropertyHouseModalPage : ContentPage
{
    public AddSystemAgentPropertyHouseModalPage(PropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}