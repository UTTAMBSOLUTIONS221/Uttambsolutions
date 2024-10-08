using Maqaoplus.ViewModels.BillsandPayments;

namespace Maqaoplus.Views.BillsandPayments;
public partial class OwnerPropertyHousesPaymentsPage : ContentPage
{
    private PropertyHousesBillsandPaymentsViewModel _viewModel;
    public OwnerPropertyHousesPaymentsPage(PropertyHousesBillsandPaymentsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadOwnerPaymentItemsCommand.CanExecute(null))
        {
            _viewModel.LoadOwnerPaymentItemsCommand.Execute(null);
        }
    }
}