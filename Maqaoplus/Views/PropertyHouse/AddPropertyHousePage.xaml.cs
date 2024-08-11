using Maqaoplus.ViewModels.PropertyHouse;
namespace Maqaoplus.Views.PropertyHouse;

public partial class AddPropertyHousePage : ContentPage
{
    public AddPropertyHousePage(AddPropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AddPropertyHouseViewModel viewModel && viewModel.LoadItemsCommand.CanExecute(null))
        {
            viewModel.LoadItemsCommand.Execute(null);
        }
    }
}