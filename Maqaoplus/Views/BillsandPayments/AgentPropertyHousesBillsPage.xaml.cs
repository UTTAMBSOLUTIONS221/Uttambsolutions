using Maqaoplus.ViewModels.BillsandPayments;

namespace Maqaoplus.Views.BillsandPayments;

public partial class AgentPropertyHousesBillsPage : ContentPage
{
    private PropertyHousesBillsandPaymentsViewModel _viewModel;

    public AgentPropertyHousesBillsPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHousesBillsandPaymentsViewModel(serviceProvider);
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