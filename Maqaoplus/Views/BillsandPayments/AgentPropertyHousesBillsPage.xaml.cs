using Maqaoplus.ViewModels.BillsandPayments;

namespace Maqaoplus.Views.BillsandPayments;
public partial class AgentPropertyHousesBillsPage : ContentPage
{
    private PropertyHousesBillsandPaymentsViewModel _viewModel;
    public AgentPropertyHousesBillsPage(PropertyHousesBillsandPaymentsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadAgentBillItemsCommand.CanExecute(null))
        {
            _viewModel.LoadAgentBillItemsCommand.Execute(null);
        }
    }
}