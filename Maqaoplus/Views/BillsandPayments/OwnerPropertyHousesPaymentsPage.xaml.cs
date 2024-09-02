using Maqaoplus.ViewModels.BillsandPayments;

namespace Maqaoplus.Views.BillsandPayments;

public partial class OwnerPropertyHousesPaymentsPage : ContentPage
{
    private PropertyHousesBillsandPaymentsViewModel _viewModel;

    public OwnerPropertyHousesPaymentsPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHousesBillsandPaymentsViewModel(serviceProvider);
        this.BindingContext = _viewModel;

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