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
                    Confirmpasswords = ConfirmPassword,
                    Datecreated = DateTime.Now,
                    Datemodified = DateTime.Now,
                    Lastlogin = DateTime.Now,
                    Loginstatus = 1,
                    Isactive = true,
                    Isdeleted = false,
                    Isdefault = false,
                    Updateprofile = true,
                    Parentid = 0,
                    Passwordresetdate = DateTime.Now.AddDays(90),
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
        private string _systemStaffFirstNameError;
        public string SystemStaffFirstNameError
        {
            get => _systemStaffFirstNameError;
            set
            {
                _systemStaffFirstNameError = value;
                OnPropertyChanged();
            }
        }
        private string _systemStaffLastNameError;
        public string SystemStaffLastNameError
        {
            get => _systemStaffLastNameError;
            set
            {
                _systemStaffLastNameError = value;
                OnPropertyChanged();
            }
        }
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
        private string _systemStaffPhonenumberError;
        public string SystemStaffPhonenumberError
        {
            get => _systemStaffPhonenumberError;
            set
            {
                _systemStaffPhonenumberError = value;
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


        private bool IsValidInput()
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                SystemStaffFirstNameError = "First Name is required.";
                isValid = false;
            }
            else
            {
                SystemStaffFirstNameError = null;
            }
            if (string.IsNullOrWhiteSpace(LastName))
            {
                SystemStaffLastNameError = "Last Name is required.";
                isValid = false;
            }
            else
            {
                SystemStaffLastNameError = null;
            }

            if (string.IsNullOrWhiteSpace(EmailAddress))
            {
                SystemStaffEmailAddressError = "Email Address is required.";
                isValid = false;
            }
            else
            {
                SystemStaffEmailAddressError = null;
            }
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                SystemStaffPhonenumberError = "Phonenumber Name is required.";
                isValid = false;
            }
            else
            {
                SystemStaffPhonenumberError = null;
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
            if (Password == ConfirmPassword)
            {
                SystemStaffConfirmPasswordError = "Password Mismatch.";
                isValid = false;
            }
            else
            {
                SystemStaffConfirmPasswordError = null;
            }

            return isValid;
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
