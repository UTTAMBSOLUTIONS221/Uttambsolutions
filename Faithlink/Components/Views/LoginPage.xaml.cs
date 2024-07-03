using Faithlink.Services;
using Microsoft.Maui.Controls;

namespace Faithlink.Components.Views
{
    public partial class LoginPage : ContentPage
    {
        private readonly AuthenticationService _authService;

        public LoginPage(AuthenticationService authService)
        {
            InitializeComponent();
            _authService = authService;
        }

        private async void OnLoginClicked(object sender, EventArgs e)
        {
            var username = UsernameEntry?.Text;
            var password = PasswordEntry?.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                await DisplayAlert("Error", "Please enter both username and password", "OK");
                return;
            }

            try
            {
                var resp = await _authService.Validateuser(username, password);
                if (resp.RespStatus == 200 || resp.RespStatus == 0)
                {
                    await DisplayAlert("Success", "Login Successful", "OK");
                    // Example of navigating to another page:
                    await Navigation.PushAsync(new MainPage());
                }
                else
                {
                    await DisplayAlert("Error", "Invalid username or password", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}
