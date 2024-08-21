using Maqaoplus.ViewModels.OwnerBillsandPayments;
namespace Maqaoplus.Views.OwnerBillsandPayments;

public partial class PropertyHousesOwnerPaymentsPage : ContentPage
{
    private PropertyHousesOwnerPaymentsViewModel _viewModel;

    public PropertyHousesOwnerPaymentsPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHousesOwnerPaymentsViewModel(serviceProvider);
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