using Maqaoplus.ViewModels.Startup;
namespace Maqaoplus.Views.Startup;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}