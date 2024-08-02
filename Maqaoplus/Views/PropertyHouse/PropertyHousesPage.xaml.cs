using Maqaoplus.ViewModels.PropertyHouse;
namespace Maqaoplus.Views.PropertyHouse;

public partial class PropertyHousesPage : ContentPage
{
    private PropertyHouseViewModel _viewModel;

    public PropertyHousesPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHouseViewModel(serviceProvider);
        this.BindingContext = _viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadItemsCommand.CanExecute(null))
        {
            _viewModel.LoadItemsCommand.Execute(null);
            // You can directly call the method if needed
            // await _viewModel.LoadItems();
        }
    }
}
