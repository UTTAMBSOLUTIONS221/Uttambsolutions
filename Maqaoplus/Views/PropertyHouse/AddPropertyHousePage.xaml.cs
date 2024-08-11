using Maqaoplus.ViewModels.PropertyHouse;
namespace Maqaoplus.Views.PropertyHouse;

public partial class AddPropertyHousePage : ContentPage
{
    public AddPropertyHousePage(AddPropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}