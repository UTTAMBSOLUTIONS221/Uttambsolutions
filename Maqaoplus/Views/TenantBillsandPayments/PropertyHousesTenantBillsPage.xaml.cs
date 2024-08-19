using Maqaoplus.ViewModels.TenantBillsandPayments;
namespace Maqaoplus.Views.TenantBillsandPayments;

public partial class PropertyHousesTenantBillsPage : ContentPage
{
    private PropertyHousesTenantBillsViewModel _viewModel;

    public PropertyHousesTenantBillsPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHousesTenantBillsViewModel(serviceProvider);
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