namespace Faithlink.Components.Views;

public partial class LoginPage : ContentPage
{
        private AuthenticationService _authService;

    public LoginPage()
    {
        InitializeComponent();
        _authService = new AuthenticationService();
    }

    private async void OnLoginClicked(object sender, EventArgs e)
    {
        var username = UsernameEntry.Text;
        var password = PasswordEntry.Text;

        if (await _authService.LoginAsync(username, password))
        {
            // Navigate to next page or perform actions upon successful login
            await DisplayAlert("Success", "Login Successful", "OK");
            // Example of navigating to another page:
            // await Navigation.PushAsync(new MainPage());
        }
        else
        {
            // Handle unsuccessful login
            await DisplayAlert("Error", "Invalid username or password", "OK");
        }
    }
}