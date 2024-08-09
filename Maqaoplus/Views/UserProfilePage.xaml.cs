using Maqaoplus.ViewModels;
namespace Maqaoplus.Views;
public partial class UserProfilePage : ContentPage
{
    private UserProfilePageViewModel _viewModel;

    public UserProfilePage(UserProfilePageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is UserProfilePageViewModel viewModel && viewModel.LoadCurrentUserCommand.CanExecute(null))
        {
            viewModel.LoadCurrentUserCommand.Execute(null);
        }
    }
}