using Parceldrop.ViewModels.Startup;

namespace Parceldrop.Views;
public partial class UpdateUserProfilePage : ContentPage
{
    private LoginPageViewModel _viewModel;

    public UpdateUserProfilePage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is LoginPageViewModel viewModel && viewModel.LoadCurrentUserCommand.CanExecute(null))
        {
            viewModel.LoadCurrentUserCommand.Execute(null);
        }
    }
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
    }
}