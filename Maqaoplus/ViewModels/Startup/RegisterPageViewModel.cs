using DBL.Entities;
using Maqaoplus.Views.Startup;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;


namespace Maqaoplus.ViewModels.Startup
{
    public class RegisterPageViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _emailAddress;
        private string _phoneNumber;
        private string _password;
        private string _confirmPassword;
        private bool _isProcessing;

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Services.ServiceProvider _serviceProvider;

        public RegisterPageViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged();
                ((Command)SignUpCommand).ChangeCanExecute();
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged();
                ((Command)SignUpCommand).ChangeCanExecute();
            }
        }

        public string EmailAddress
        {
            get => _emailAddress;
            set
            {
                _emailAddress = value;
                OnPropertyChanged();
                ((Command)SignUpCommand).ChangeCanExecute();
            }
        }

        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                OnPropertyChanged();
                ((Command)SignUpCommand).ChangeCanExecute();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
                ((Command)SignUpCommand).ChangeCanExecute();
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged();
                ((Command)SignUpCommand).ChangeCanExecute();
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged();
                ((Command)SignUpCommand).ChangeCanExecute();
            }
        }

        public ICommand SignUpCommand => new Command(async () => await OnSignUp(), () => !IsProcessing);
        public ICommand CancelCommand => new Command(async () => await OnCancel());
        public ICommand SignInCommand => new Command(async () => await OnSignIn());

        private async Task OnSignUp()
        {
            if (IsProcessing || !IsValidInput())
                return;

            try
            {
                IsProcessing = true;

                var request = new SystemStaff
                {
                    Firstname = FirstName,
                    Lastname = LastName,
                    Emailaddress = EmailAddress,
                    Phonenumber = PhoneNumber,
                    Passwords = Password,
                    Confirmpasswords = ConfirmPassword
                };

                // Call your registration service here
                var response = await _serviceProvider.CallUnAuthWebApi("/api/Account/Registerstaff", HttpMethod.Post, request);
                if (response.StatusCode == 200)
                {
                    await Shell.Current.GoToAsync("//LoginPage");
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

        private bool IsValidInput()
        {
            // Implement your input validation logic here
            return !string.IsNullOrWhiteSpace(FirstName) &&
                   !string.IsNullOrWhiteSpace(LastName) &&
                   !string.IsNullOrWhiteSpace(EmailAddress) &&
                   !string.IsNullOrWhiteSpace(PhoneNumber) &&
                   !string.IsNullOrWhiteSpace(Password) &&
                   Password == ConfirmPassword;
        }

        private async Task OnCancel()
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }

        private async Task OnSignIn()
        {
            await Shell.Current.GoToAsync(nameof(LoginPage));
        }
    }
}
