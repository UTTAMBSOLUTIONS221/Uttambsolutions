using Mainapp.ViewModels.User;
namespace Mainapp.Pages.Users;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }
}