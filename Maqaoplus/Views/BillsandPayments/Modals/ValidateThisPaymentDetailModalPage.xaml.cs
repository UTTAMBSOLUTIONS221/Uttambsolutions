using Maqaoplus.ViewModels.BillsandPayments;

namespace Maqaoplus.Views.BillsandPayments.Modals;

public partial class ValidateThisPaymentDetailModalPage : ContentPage
{
    public ValidateThisPaymentDetailModalPage(PropertyHousesBillsandPaymentsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}