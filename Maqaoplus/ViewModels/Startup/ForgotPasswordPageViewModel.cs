using DBL.Entities;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Newtonsoft.Json;
#if ANDROID
using Android.Provider;
using Android.Content;
#endif
namespace Maqaoplus.ViewModels.Startup
{
    public class ForgotPasswordPageViewModel : INotifyPropertyChanged
    {
        private string _emailAddress;
        private string _passwords;
        private string _confirmpasswords;
        private Forgotpassword _forgotPasswordData;
        private bool _isProcessing;
        private bool _isPasswordHidden;
        private string _passwordIconSource;
        private bool _isConfirmPasswordHidden;
        private string _confirmPasswordIconSource;
        private bool _isPasswordInputHidden;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        public ICommand TogglePasswordVisibilityCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Services.ServiceProvider _serviceProvider;

        public ForgotPasswordPageViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            ForgotPasswordData = new Forgotpassword();
            IsPasswordHidden = true;
            IsConfirmPasswordHidden = true;
            TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);
            ToggleConfirmPasswordVisibilityCommand = new Command(ToggleConfirmPasswordVisibility);
            IsPasswordInputHidden = false;
            TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public Forgotpassword ForgotPasswordData
        {
            get => _forgotPasswordData;
            set
            {
                _forgotPasswordData = value;
                OnPropertyChanged();
            }
        }
        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                _emailAddress = value;
                OnPropertyChanged();
                ((Command)ForgotPasswordCommand).ChangeCanExecute();
            }
        }
        public string Passwords
        {
            get => _passwords;
            set
            {
                _passwords = value;
                OnPropertyChanged();
                ((Command)ForgotPasswordCommand).ChangeCanExecute();
            }
        }
        public string Confirmpasswords
        {
            get => _confirmpasswords;
            set
            {
                _confirmpasswords = value;
                OnPropertyChanged();
                ((Command)ForgotPasswordCommand).ChangeCanExecute();
            }
        }
        public bool IsPasswordHidden
        {
            get => _isPasswordHidden;
            set
            {
                _isPasswordHidden = value;
                OnPropertyChanged(nameof(IsPasswordHidden));
                PasswordIconSource = _isPasswordHidden ? "unvisible.png" : "visible.png";
            }
        }

        public string PasswordIconSource
        {
            get => _passwordIconSource;
            set
            {
                _passwordIconSource = value;
                OnPropertyChanged(nameof(PasswordIconSource));
            }
        }
        public bool IsConfirmPasswordHidden
        {
            get => _isConfirmPasswordHidden;
            set
            {
                _isConfirmPasswordHidden = value;
                OnPropertyChanged(nameof(IsConfirmPasswordHidden));
                ConfirmPasswordIconSource = _isConfirmPasswordHidden ? "unvisible.png" : "visible.png";
            }
        }

        public string ConfirmPasswordIconSource
        {
            get => _confirmPasswordIconSource;
            set
            {
                _confirmPasswordIconSource = value;
                OnPropertyChanged(nameof(ConfirmPasswordIconSource));
            }
        }
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged();
                ((Command)ForgotPasswordCommand).ChangeCanExecute();
            }
        }

        public bool IsPasswordInputHidden
        {
            get => _isPasswordInputHidden;
            set
            {
                _isPasswordInputHidden = value;
                OnPropertyChanged(nameof(IsPasswordInputHidden));
            }
        }

        public ICommand ForgotPasswordCommand => new Command(async () => await ForgotPasswordAsync(), () => !IsProcessing);

        private string _systemStaffEmailAddressError;
        public string SystemStaffEmailAddressError
        {
            get => _systemStaffEmailAddressError;
            set
            {
                _systemStaffEmailAddressError = value;
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
        private string _systemStaffConfirmPasswordError;
        public string SystemStaffConfirmPasswordError
        {
            get => _systemStaffConfirmPasswordError;
            set
            {
                _systemStaffConfirmPasswordError = value;
                OnPropertyChanged();
            }
        }

        private void TogglePasswordVisibility()
        {
            IsPasswordHidden = !IsPasswordHidden;
        }
        private void ToggleConfirmPasswordVisibility()
        {
            IsConfirmPasswordHidden = !IsConfirmPasswordHidden;
        }
        private async Task ForgotPasswordAsync()
        {
            IsProcessing = true;

            if (!IsValidInput())
            {
                IsProcessing = false;
                return;
            }
#if ANDROID
            // Fetch Android ID on Android
            string androidId = Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Settings.Secure.AndroidId);
#else
                // Provide a default or placeholder value for other platforms
                string androidId = "NotApplicable"; 
#endif
            try
            {
                IsProcessing = true;
                ForgotPasswordData.Androidid = androidId;
                // Call your registration service here
                var response = await _serviceProvider.CallUnAuthWebApi("/api/Account/Forgotstaffpassword", HttpMethod.Post, ForgotPasswordData);
                if (response.StatusCode == 200)
                {
                    ForgotPasswordData = JsonConvert.DeserializeObject<Forgotpassword>(response.Data.ToString());
                    IsPasswordInputHidden = true;
                    if (ForgotPasswordData.Passwordstatus == "Passwordupdated")
                    {
                        await Shell.Current.GoToAsync("//LoginPage");
                    }
                }
                else if (response.StatusCode == 1)
                {
                    await Shell.Current.DisplayAlert("Warning", "Something went wrong. Contact Admin!", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Sever error occured. Kindly Contact Admin!", "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsProcessing = false;
            }
        }
        private bool IsValidEmail(string email)
        {
            // Define a simple email regex pattern
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

            return Regex.IsMatch(email, emailPattern);
        }
        private bool IsValidInput()
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(ForgotPasswordData.Emailaddress))
            {
                SystemStaffEmailAddressError = "Email Address is required.";
                isValid = false;
            }
            else if (!IsValidEmail(ForgotPasswordData.Emailaddress))
            {
                SystemStaffEmailAddressError = "Invalid email address format.";
                isValid = false;
            }
            else
            {
                SystemStaffEmailAddressError = null;
            }
            if (ForgotPasswordData.Userid > 0)
            {
                if (string.IsNullOrWhiteSpace(ForgotPasswordData.Passwords))
                {
                    SystemStaffPasswordError = "Required.";
                    isValid = false;
                }
                else
                {
                    SystemStaffPasswordError = null;
                }
                if (string.IsNullOrWhiteSpace(ForgotPasswordData.Confirmpasswords))
                {
                    SystemStaffConfirmPasswordError = "Required.";
                    isValid = false;
                }
                else
                {
                    SystemStaffConfirmPasswordError = null;
                }
                if (ForgotPasswordData.Passwords != ForgotPasswordData.Confirmpasswords)
                {
                    SystemStaffConfirmPasswordError = "Required.";
                    isValid = false;
                }
                else
                {
                    SystemStaffConfirmPasswordError = null;
                }
            }

            return isValid;
        }
    }
}
