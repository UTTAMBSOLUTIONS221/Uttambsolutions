using Maqaoplus.ViewModels;

namespace Maqaoplus.Views;
public partial class UserProfilePage : ContentPage
{
    public partial class MainPage : ContentPage
    {
        private UserProfilePageViewModel _viewModel;
        public MainPage(UserProfilePageViewModel viewModel)
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
}
