using Maqaoplus.ViewModels.BillsandPayments;

namespace Maqaoplus.Views.BillsandPayments;
public partial class AgentPropertyHousesPaymentsPage : ContentPage
{
    private PropertyHousesBillsandPaymentsViewModel _viewModel;
    public AgentPropertyHousesPaymentsPage(PropertyHousesBillsandPaymentsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
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