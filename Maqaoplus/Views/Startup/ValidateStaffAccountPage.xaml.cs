using Maqaoplus.ViewModels.Startup;
namespace Maqaoplus.Views.Startup;
public partial class ValidateStaffAccountPage : ContentPage
{
    private ValidateStaffAccountPageViewModel _viewModel;

    public ValidateStaffAccountPage(ValidateStaffAccountPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
}