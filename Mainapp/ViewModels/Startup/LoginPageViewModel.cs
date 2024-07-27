using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mainapp.Constants;
using Mainapp.Services.Startup;
using Newtonsoft.Json;


namespace Mainapp.ViewModels.Startup
{
    public partial class LoginPageViewModel : BaseViewModel
    {
        private readonly IAuthenticationService bl;

        public LoginPageViewModel(IAuthenticationService authService)
        {
            bl = authService;
        }
        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;


        #region Commands
        [RelayCommand]
        public async void Login()
        {
            if (!string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password))
            {
                try
                {
                    var userDetails = await bl.Validateuser(Email, Password);
                    // Store user details locally (e.g., using Preferences)
                    string userDetailStr = JsonConvert.SerializeObject(userDetails);
                    Preferences.Set(nameof(App.UserDetails), userDetailStr);
                    App.UserDetails = userDetails.Usermodel;

                    // Example additional logic after successful login
                    await AppConstant.AddFlyoutMenusDetails();
                }
                catch (Exception ex)
                {
                    // Handle API error (e.g., show error message)
                    Console.WriteLine($"Login failed: {ex.Message}");
                }
            }
        }
        #endregion
    }
}
