using Maqaoplus.ViewModels.PropertyHouseTenants;

namespace Maqaoplus.Views.PropertyHouseTenants;


public partial class AgentPropertyHousesRoomTenantsPage : ContentPage
{
    private PropertyHousesRoomTenantsViewModel _viewModel;

    public AgentPropertyHousesRoomTenantsPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHousesRoomTenantsViewModel(serviceProvider);
        this.BindingContext = _viewModel;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadAgentItemsCommand.CanExecute(null))
        {
            _viewModel.LoadAgentItemsCommand.Execute(null);
        }
    }
}