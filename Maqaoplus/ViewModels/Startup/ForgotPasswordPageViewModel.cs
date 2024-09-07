using DBL.Entities;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;

#if ANDROID
using Android.Provider;
using Android.Content;
#endif
namespace Maqaoplus.ViewModels.Startup
{
    public class ForgotPasswordPageViewModel : INotifyPropertyChanged
    {
        private string _emailAddress;
        private bool _isProcessing;
        private bool _isPasswordHidden;
        private string _passwordIconSource;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        public ICommand TogglePasswordVisibilityCommand { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Services.ServiceProvider _serviceProvider;

        public ForgotPasswordPageViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
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
        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged();
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

        private void TogglePasswordVisibility()
        {
            IsPasswordHidden = !IsPasswordHidden;
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

                var request = new Forgotpassword
                {
                    Emailaddress = EmailAddress,
                    Androidid = androidId
                };

                // Call your registration service here
                var response = await _serviceProvider.CallUnAuthWebApi("/api/Account/Forgotstaffpassword", HttpMethod.Post, request);
                if (response.StatusCode == 200)
                {
                    await Shell.Current.DisplayAlert("Success", "User Found", "OK");

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
            if (string.IsNullOrWhiteSpace(EmailAddress))
            {
                SystemStaffEmailAddressError = "Email Address is required.";
                isValid = false;
            }
            else if (!IsValidEmail(EmailAddress))
            {
                SystemStaffEmailAddressError = "Invalid email address format.";
                isValid = false;
            }
            else
            {
                SystemStaffEmailAddressError = null;
            }
            return isValid;
        }
    }
}
