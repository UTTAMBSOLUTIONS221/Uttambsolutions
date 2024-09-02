using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse;


public partial class AgentPropertyHousesPage : ContentPage
{
    private PropertyHouseViewModel _viewModel;

    public AgentPropertyHousesPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHouseViewModel(serviceProvider);
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