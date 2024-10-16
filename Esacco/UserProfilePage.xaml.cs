using Esacco.ViewModels;

namespace Esacco;
public partial class UserProfilePage : ContentPage
{
    private UserManagementViewModel _viewModel;
    public UserProfilePage(UserManagementViewModel viewModel)
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
