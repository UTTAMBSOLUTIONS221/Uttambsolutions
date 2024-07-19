using App.ViewModels.User;
namespace App.Pages.Users;
public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();

        this.BindingContext = viewModel;
    }
}