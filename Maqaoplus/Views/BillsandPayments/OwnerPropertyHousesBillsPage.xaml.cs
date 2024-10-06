using Maqaoplus.ViewModels.BillsandPayments;

namespace Maqaoplus.Views.BillsandPayments;


public partial class OwnerPropertyHousesBillsPage : ContentPage
{
    private PropertyHousesBillsandPaymentsViewModel _viewModel;
    public OwnerPropertyHousesBillsPage(PropertyHousesBillsandPaymentsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadOwnerBillItemsCommand.CanExecute(null))
        {
            _viewModel.LoadOwnerBillItemsCommand.Execute(null);
        }
    }
}