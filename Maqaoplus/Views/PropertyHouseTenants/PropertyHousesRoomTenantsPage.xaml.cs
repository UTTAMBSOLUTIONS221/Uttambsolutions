using Maqaoplus.ViewModels.PropertyHouseTenants;
namespace Maqaoplus.Views.PropertyHouseTenants;

public partial class PropertyHousesRoomTenantsPage : ContentPage
{
    private PropertyHousesRoomTenantsViewModel _viewModel;

    public PropertyHousesRoomTenantsPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHousesRoomTenantsViewModel(serviceProvider);
        this.BindingContext = _viewModel;

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