using Maqaoplus.ViewModels.OwnerBillsandPayments;

namespace Maqaoplus.Views.OwnerBillsandPayments.Modals;

public partial class ValidateThisPaymentDetailModalPage : ContentPage
{
    public ValidateThisPaymentDetailModalPage(PropertyHousesOwnerPaymentsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}