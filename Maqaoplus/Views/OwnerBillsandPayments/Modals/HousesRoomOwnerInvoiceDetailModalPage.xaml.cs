using Maqaoplus.ViewModels.OwnerBillsandPayments;

namespace Maqaoplus.Views.OwnerBillsandPayments.Modals;

public partial class HousesRoomOwnerInvoiceDetailModalPage : ContentPage
{
    public HousesRoomOwnerInvoiceDetailModalPage(PropertyHousesOwnerBillsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}