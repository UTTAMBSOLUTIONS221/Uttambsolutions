using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Faithlink.Models;
using Faithlink.Services;
using Newtonsoft.Json;

namespace Faithlink.ViewModels.Startup
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

                    // Process user details as needed
                    userDetails.Usermodel.Fullname = "Test User Name"; // Example modification

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
