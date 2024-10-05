using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse;

public partial class AgentPropertyHousesPage : ContentPage
{
    private PropertyHouseViewModel _viewModel;

    public AgentPropertyHousesPage()
    {
        InitializeComponent();

        _viewModel = new PropertyHouseViewModel();

        BindingContext = _viewModel;
    }

    public AgentPropertyHousesPage(PropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Ensure the command can be executed
        if (_viewModel.LoadAgentItemsCommand.CanExecute(null))
        {
            _viewModel.LoadAgentItemsCommand.Execute(null);
        }
    }
}
