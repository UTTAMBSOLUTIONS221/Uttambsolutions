
using Esacco.ViewModels;

namespace Esacco;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
    }
}

public partial class RegisterPage : ContentPage
{
    public RegisterPage(UserManagementViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}