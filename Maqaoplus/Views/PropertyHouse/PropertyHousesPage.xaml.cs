using Maqaoplus.ViewModels.PropertyHouse;
namespace Maqaoplus.Views.PropertyHouse;

public partial class PropertyHousesPage : ContentPage
{
    public PropertyHousesPage(PropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}