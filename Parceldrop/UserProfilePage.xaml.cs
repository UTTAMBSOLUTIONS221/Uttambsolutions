using Parceldrop.ViewModels.Startup;

namespace Parceldrop.Views;
public partial class UserProfilePage : ContentPage
{
    private LoginPageViewModel _viewModel;
    public UserProfilePage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadCurrentUserCommand.CanExecute(null))
        {
            _viewModel.LoadCurrentUserCommand.Execute(null);
        }
    }
}
