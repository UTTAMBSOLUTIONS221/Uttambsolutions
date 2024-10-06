using Maqaoplus.ViewModels.BillsandPayments;

namespace Maqaoplus.Views.BillsandPayments;

public partial class AgentPropertyHousesPaymentsPage : ContentPage
{
    private PropertyHousesBillsandPaymentsViewModel _viewModel;

    public AgentPropertyHousesPaymentsPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHousesBillsandPaymentsViewModel(serviceProvider);
        this.BindingContext = _viewModel;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadAgentPaymentItemsCommand.CanExecute(null))
        {
            _viewModel.LoadAgentPaymentItemsCommand.Execute(null);
        }
    }
}