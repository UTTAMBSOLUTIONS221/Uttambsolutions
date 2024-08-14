using Maqaoplus.ViewModels.Startup;
namespace Maqaoplus.Views;

public partial class ConfirmPaymentDetailModalPage : ContentPage
{
    public ConfirmPaymentDetailModalPage(ValidateStaffAccountPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}