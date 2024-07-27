using Mainapp.Constants;
using Mainapp.Entities.Startup;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Mainapp.ViewModels.Startup
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private readonly Services.ServiceProvider _serviceProvider;

        public LoginPageViewModel(Services.ServiceProvider serviceProvider)
        {
            UserName = "info@uttambsolutions.com";
            Password = "Password123!";
            IsProcessing = false;

            LoginCommand = new Command(async () =>
            {
                if (IsProcessing) return;

                if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password)) return;

                IsProcessing = true;
                await Login();
                IsProcessing = false;
            });

            _serviceProvider = serviceProvider;
        }

        async Task Login()
        {
            try
            {
                var request = new Userloginmodel
                {
                    username = UserName,
                    password = Password,
                };
                var response = await _serviceProvider.Authenticate(request);
                if (response.StatusCode == 200)
                {
                    // Store user details locally (e.g., using Preferences)
                    string userDetailStr = JsonConvert.SerializeObject(response);
                    Preferences.Set(nameof(App.UserDetails), userDetailStr);
                    App.UserDetails = response.Usermodel;

                    // Example additional logic after successful login
                    await AppConstant.AddFlyoutMenusDetails();
                }
                else
                {
                    await Shell.Current.DisplayAlert("Uttamb Solutions", response.StatusMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Uttamb Solutions", ex.Message, "OK");
            }
        }

        private string userName;
        private string password;
        private bool isProcessing;

        public string UserName
        {
            get { return userName; }
            set { userName = value; OnPropertyChanged(); }
        }

        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }

        public bool IsProcessing
        {
            get { return isProcessing; }
            set { isProcessing = value; OnPropertyChanged(); }
        }

        public ICommand LoginCommand { get; set; }
    }
}
