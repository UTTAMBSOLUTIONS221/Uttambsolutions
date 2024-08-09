using Maqaoplus.ViewModels.Startup;
namespace Maqaoplus.Views.Startup;

public partial class ForgotPasswordPage : ContentPage
{
    public ForgotPasswordPage(ForgotPasswordPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}