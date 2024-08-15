using DBL.Entities;
using DBL.Enum;
using Maqaoplus.Constants;
using Maqaoplus.Views;
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
            ForgotPasswordCommand = new Command(OnForgotPassword);
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
        public ICommand ForgotPasswordCommand { get; }


        private string _systemStaffUserNameError;
        public string SystemStaffUserNameError
        {
            get => _systemStaffUserNameError;
            set
            {
                _systemStaffUserNameError = value;
                OnPropertyChanged();
            }
        }

        private string _systemStaffPasswordError;
        public string SystemStaffPasswordError
        {
            get => _systemStaffPasswordError;
            set
            {
                _systemStaffPasswordError = value;
                OnPropertyChanged();
            }
        }

        private async Task LoginAsync()
        {
            IsProcessing = true;
            await Task.Delay(500);
            if (!IsValidInput())
            {
                IsProcessing = false;
                return;
            }

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
                    App.UserDetails = response;
                    if (response.Usermodel.Updateprofile)
                    {
                        await Shell.Current.GoToAsync(nameof(UserProfilePage));
                    }
                    else if (response.Usermodel.Loginstatus == (int)UserLoginStatus.VerifyAccount)
                    {
                        var encodedStaffId = Uri.EscapeDataString(response.Usermodel.Userid.ToString());
                        System.Diagnostics.Debug.WriteLine($"Navigating to ValidateStaffAccountPage with UserId={encodedStaffId}");
                        await Shell.Current.GoToAsync($"ValidateStaffAccountPage?UserId={encodedStaffId}");
                    }
                    else
                    {
                        // Store user details locally (e.g., using Preferences)
                        string userDetailStr = JsonConvert.SerializeObject(response);
                        Preferences.Set(nameof(App.UserDetails), userDetailStr);

                        // Example additional logic after successful login
                        await AppConstant.AddFlyoutMenusDetails();
                    }
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
        private bool IsValidInput()
        {
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(UserName))
            {
                SystemStaffUserNameError = "Email Address is required.";
                isValid = false;
            }
            else
            {
                SystemStaffUserNameError = null;
            }

            if (string.IsNullOrWhiteSpace(Password))
            {
                SystemStaffPasswordError = "Password is required.";
                isValid = false;
            }
            else
            {
                SystemStaffPasswordError = null;
            }


            return isValid;
        }


        private async void OnRegister()
        {
            SystemStaff systemStaff = null;
            await Shell.Current.GoToAsync(nameof(RegisterPage));
        }
        private async void OnForgotPassword()
        {
            await Shell.Current.GoToAsync(nameof(ForgotPasswordPage));
        }
    }
}
