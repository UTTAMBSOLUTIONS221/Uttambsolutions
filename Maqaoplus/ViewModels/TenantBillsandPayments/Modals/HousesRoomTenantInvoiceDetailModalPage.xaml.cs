namespace Maqaoplus.ViewModels.TenantBillsandPayments.Modals;

public partial class HousesRoomTenantInvoiceDetailModalPage : ContentPage
{
    public HousesRoomTenantInvoiceDetailModalPage(PropertyHousesTenantBillsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}