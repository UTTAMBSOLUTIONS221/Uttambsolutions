using Maqaoplus.ViewModels;

namespace Maqaoplus.Views;
public partial class UserProfilePage : ContentPage
{
    private UserProfilePageViewModel _viewModel;
    public UserProfilePage(UserProfilePageViewModel viewModel)
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
