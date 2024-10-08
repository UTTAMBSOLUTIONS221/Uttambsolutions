using Maqaoplus.ViewModels.PropertyHouse;
namespace Maqaoplus.Views.PropertyHouse;
public partial class PropertyHousesPage : ContentPage
{
    private PropertyHouseViewModel _viewModel;
    public PropertyHousesPage(PropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadItemsCommand.CanExecute(null))
        {
            _viewModel.LoadItemsCommand.Execute(null);
        }
    }
}
