using Maqaoplus.ViewModels.PropertyHouseTenants;
namespace Maqaoplus.Views.PropertyHouseTenants;
public partial class PropertyHousesRoomTenantsPage : ContentPage
{
    private PropertyHousesRoomTenantsViewModel _viewModel;
    public PropertyHousesRoomTenantsPage(PropertyHousesRoomTenantsViewModel viewModel)
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