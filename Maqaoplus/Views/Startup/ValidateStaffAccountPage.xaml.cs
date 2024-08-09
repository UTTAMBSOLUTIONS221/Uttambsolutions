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
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ValidateStaffAccountPageViewModel viewModel && viewModel.LoadCurrentUserCommand.CanExecute(null))
        {
            viewModel.LoadCurrentUserCommand.Execute(null);
        }
    }
}