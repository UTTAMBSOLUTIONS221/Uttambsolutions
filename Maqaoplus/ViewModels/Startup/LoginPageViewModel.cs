using DBL.Entities;
using Maqaoplus.Constants;
using Maqaoplus.Views.Startup;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.Startup
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Services.ServiceProvider _serviceProvider;

        public LoginPageViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoginCommand = new Command(async () => await LoginAsync(), () => !IsProcessing);
            RegisterCommand = new Command(OnRegister);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string _userName;
        private string _password;
        private bool _isProcessing;

        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged();
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged();
                ((Command)LoginCommand).ChangeCanExecute();
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        private async Task LoginAsync()
        {
            if (IsProcessing || string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password))
                return;

            try
            {
                IsProcessing = true;

                var request = new Userloginmodel
                {
                    username = UserName,
                    password = Password
                };

                var response = await _serviceProvider.Authenticate(request);

                if (response.RespStatus == 200)
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
                    await Shell.Current.DisplayAlert("Uttamb Solutions", response.RespMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Uttamb Solutions", ex.Message, "OK");
            }
            finally
            {
                IsProcessing = false;
            }
        }
        private async void OnRegister()
        {
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }
    }
}
