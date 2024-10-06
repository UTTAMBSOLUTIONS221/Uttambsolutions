using Maqaoplus.ViewModels;
namespace Maqaoplus.Views;
public partial class UpdateUserProfilePage : ContentPage
{
    private UserProfilePageViewModel _viewModel;

    public UpdateUserProfilePage(UserProfilePageViewModel viewModel)
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
    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.CancelOperations();
    }
}