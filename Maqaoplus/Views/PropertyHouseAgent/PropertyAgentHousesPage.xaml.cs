using Maqaoplus.ViewModels.PropertyHouseAgent;

namespace Maqaoplus.Views.PropertyHouseAgent;
public partial class PropertyAgentHousesPage : ContentPage
{
    private PropertyHouseAgentViewModel _viewModel;

    public PropertyAgentHousesPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHouseAgentViewModel(serviceProvider);
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