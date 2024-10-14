using Parceldrop.ViewModels.Startup;

namespace Parceldrop.Views.Startup;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }
}

public partial class RegisterPage : ContentPage
{
    public RegisterPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}