using Maqaoplus.ViewModels.Startup;
namespace Maqaoplus.Views.Startup;

public partial class RegisterPage : ContentPage
{
    public RegisterPage(RegisterPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}