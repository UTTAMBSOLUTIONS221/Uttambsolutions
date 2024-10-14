using Parceldrop.ViewModels.Startup;

namespace Parceldrop.Views.Startup;
public partial class ValidateStaffAccountPage : ContentPage
{
    public ValidateStaffAccountPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}