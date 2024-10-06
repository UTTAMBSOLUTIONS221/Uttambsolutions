using Maqaoplus.ViewModels.TenantBillsandPayments;
namespace Maqaoplus.Views.TenantBillsandPayments;

public partial class PropertyHousesTenantPaymentsPage : ContentPage
{
    private PropertyHousesTenantPaymentsViewModel _viewModel;

    public PropertyHousesTenantPaymentsPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHousesTenantPaymentsViewModel(serviceProvider);
        this.BindingContext = _viewModel;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadPaymentItemsCommand.CanExecute(null))
        {
            _viewModel.LoadPaymentItemsCommand.Execute(null);
        }
    }
}