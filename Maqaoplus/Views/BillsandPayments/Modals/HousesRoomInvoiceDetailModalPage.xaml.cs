using Maqaoplus.ViewModels.BillsandPayments;

namespace Maqaoplus.Views.BillsandPayments.Modals;


public partial class HousesRoomInvoiceDetailModalPage : ContentPage
{
    public HousesRoomInvoiceDetailModalPage(PropertyHousesBillsandPaymentsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}