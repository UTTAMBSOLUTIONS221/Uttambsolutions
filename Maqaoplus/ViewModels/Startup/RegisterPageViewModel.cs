using DBL.Entities;
using DBL.Models;
using Maqaoplus.Views.Startup;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;


namespace Maqaoplus.ViewModels.Startup
{
    public class RegisterPageViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _emailAddress;
        private string _phoneNumber;
        private string _staffDesignation;
        private string _password;
        private string _confirmPassword;
        private bool _isProcessing;
        private bool _accepttermsandcondition;
        private bool _isPasswordHidden;
        private string _passwordIconSource;
        private bool _isConfirmPasswordHidden;
        private string _confirmPasswordIconSource;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Services.ServiceProvider _serviceProvider;
        private ObservableCollection<ListModel> _systemstaffdesignation;
        public ICommand OpenPrivacyPolicyCommand => new Command<string>(async (url) =>
        {
            if (!string.IsNullOrEmpty(url))
            {
                await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
            }
        });

        public RegisterPageViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            IsPasswordHidden = true; // Default to hidden password
            IsConfirmPasswordHidden = true; // Default to hidden password
            TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);
            ToggleConfirmPasswordVisibilityCommand = new Command(ToggleConfirmPasswordVisibility);
            Systemstaffdesignation = new ObservableCollection<ListModel>
            {
                new ListModel { Value = "Owner", Text = "Owner" },
                new ListModel { Value = "Agent", Text = "Agent" },
                new ListModel { Value = "Tenant", Text = "Tenant" },
            };
        }
        public ObservableCollection<ListModel> Systemstaffdesignation
        {
            get => _systemstaffdesignation;
            set
            {
                _systemstaffdesignation = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedStaffdesignation;
        public ListModel SelectedStaffdesignation
        {
            get => _selectedStaffdesignation;
            set
            {
                _selectedStaffdesignation = value;
                if (value != null)
                {
                    StaffDesignation = value.Value?.ToString();
                }
                OnPropertyChanged();
            }
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
        public string StaffDesignation
        {
            get => _staffDesignation;
            set
            {
                _staffDesignation = value;
                OnPropertyChanged();
                ((Command)SignUpCommand).ChangeCanExecute();
            }
        }
        public bool Accepttermsandcondition
        {
            get => _accepttermsandcondition;
            set
            {
                _accepttermsandcondition = value;
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

        public ICommand TogglePasswordVisibilityCommand { get; }
        public ICommand ToggleConfirmPasswordVisibilityCommand { get; }
        public ICommand SignUpCommand => new Command(async () => await OnSignUp(), () => !IsProcessing);
        public ICommand CancelCommand => new Command(async () => await OnCancel());
        public ICommand SignInCommand => new Command(async () => await OnSignIn());
        private void TogglePasswordVisibility()
        {
            IsPasswordHidden = !IsPasswordHidden;
        }
        private void ToggleConfirmPasswordVisibility()
        {
            IsConfirmPasswordHidden = !IsConfirmPasswordHidden;
        }
        private async Task OnSignUp()
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

                var request = new SystemStaff
                {
                    Firstname = FirstName,
                    Lastname = LastName,
                    Emailaddress = EmailAddress,
                    Phonenumber = PhoneNumber,
                    Designation = StaffDesignation,
                    Passwords = Password,
                    Confirmpasswords = ConfirmPassword,
                    Datecreated = DateTime.Now,
                    Datemodified = DateTime.Now,
                    Lastlogin = DateTime.Now,
                    Loginstatus = 0,
                    Accepttermsandcondition = Accepttermsandcondition,
                    Isactive = true,
                    Isdeleted = false,
                    Isdefault = false,
                    Updateprofile = true,
                    Parentid = 0,
                    Passwordresetdate = DateTime.Now.AddDays(90),
                };

                // Call your registration service here
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/Account/Registerstaff", request);
                if (response.RespStatus == 0 || response.RespStatus == 200)
                {
                    await Shell.Current.GoToAsync("//LoginPage");
                }
                else if (response.RespStatus == 1)
                {
                    await Shell.Current.DisplayAlert("Warning", response.RespMessage, "OK");
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
        private string _systemStaffDesignationError;
        public string SystemStaffDesignationError
        {
            get => _systemStaffDesignationError;
            set
            {
                _systemStaffDesignationError = value;
                OnPropertyChanged();
            }
        }
        private string _systemStaffAccepttermsandconditionError;
        public string SystemStaffAccepttermsandconditionError
        {
            get => _systemStaffAccepttermsandconditionError;
            set
            {
                _systemStaffAccepttermsandconditionError = value;
                OnPropertyChanged();
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
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                SystemStaffFirstNameError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffFirstNameError = null;
            }
            if (string.IsNullOrWhiteSpace(LastName))
            {
                SystemStaffLastNameError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffLastNameError = null;
            }

            if (string.IsNullOrWhiteSpace(EmailAddress))
            {
                SystemStaffEmailAddressError = "Required.";
                isValid = false;
            }
            else if (!IsValidEmail(EmailAddress))
            {
                SystemStaffEmailAddressError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffEmailAddressError = null;
            }
            if (string.IsNullOrWhiteSpace(PhoneNumber))
            {
                SystemStaffPhonenumberError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffPhonenumberError = null;
            }
            if (string.IsNullOrWhiteSpace(Password))
            {
                SystemStaffPasswordError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffPasswordError = null;
            }
            if (string.IsNullOrWhiteSpace(ConfirmPassword))
            {
                SystemStaffConfirmPasswordError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffConfirmPasswordError = null;
            }
            if (Password != ConfirmPassword)
            {
                SystemStaffConfirmPasswordError = "Required.";
                isValid = false;
            }
            else
            {
                SystemStaffConfirmPasswordError = null;
            }
            if (string.IsNullOrWhiteSpace(StaffDesignation))
            {
                SystemStaffDesignationError = "Required!.";
                isValid = false;
            }
            else
            {
                SystemStaffDesignationError = null;
            }
            if (!Accepttermsandcondition)
            {
                SystemStaffAccepttermsandconditionError = "Required!.";
                isValid = false;
            }
            else
            {
                SystemStaffAccepttermsandconditionError = null;
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
