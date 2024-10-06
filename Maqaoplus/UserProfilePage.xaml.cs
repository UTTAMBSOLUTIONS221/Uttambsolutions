using Maqaoplus.ViewModels;
namespace Maqaoplus.Views;
public partial class UserProfilePage : ContentPage
{
    private UserProfilePageViewModel _viewModel;

    public UserProfilePage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new UserProfilePageViewModel(serviceProvider);
        this.BindingContext = _viewModel;

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
