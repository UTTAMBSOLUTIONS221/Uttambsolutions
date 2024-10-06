using Maqaoplus.ViewModels.TenantBillsandPayments;
namespace Maqaoplus.Views.TenantBillsandPayments.Modals;

public partial class HousesRoomTenantInvoiceDetailModalPage : ContentPage
{
    public HousesRoomTenantInvoiceDetailModalPage(PropertyHousesTenantBillsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}