using DBL;
using DBL.Entities;
using DBL.Enum;
using DBL.Models;
using Newtonsoft.Json;
using Parceldrop.Constants;
using Parceldrop.Views.Startup;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Parceldrop.ViewModels.Startup;
public class LoginPageViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
    public ICommand OpenPrivacyPolicyCommand => new Command<string>(async (url) =>
    {
        if (!string.IsNullOrEmpty(url))
        {
            await Browser.OpenAsync(url, BrowserLaunchMode.SystemPreferred);
        }
    });

    private readonly BL _bl;
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

    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value;
            OnPropertyChanged();
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value;
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
        }
    }

    public string PhoneNumber
    {
        get => _phoneNumber;
        set
        {
            _phoneNumber = value;
            OnPropertyChanged();
        }
    }
    public string StaffDesignation
    {
        get => _staffDesignation;
        set
        {
            _staffDesignation = value;
            OnPropertyChanged();
        }
    }
    public bool Accepttermsandcondition
    {
        get => _accepttermsandcondition;
        set
        {
            _accepttermsandcondition = value;
            OnPropertyChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
        }
    }

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            _confirmPassword = value;
            OnPropertyChanged();
        }
    }
    private string _userName;
    public string UserName
    {
        get => _userName;
        set
        {
            _userName = value;
            OnPropertyChanged();
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
    private StaffDetailData _systemStaffTenantData;
    public StaffDetailData SystemStaffTenantData
    {
        get => _systemStaffTenantData;
        set
        {
            _systemStaffTenantData = value;
            OnPropertyChanged();
        }
    }


    private SystemStaff _staffData;
    public SystemStaff StaffData
    {
        get => _staffData;
        set
        {
            _staffData = value;
            OnPropertyChanged();
        }
    }

    private UsermodeldataResponce _usermodeldataResponcedata;
    public UsermodeldataResponce UsermodeldataResponcedata
    {
        get => _usermodeldataResponcedata;
        set
        {
            _usermodeldataResponcedata = value;
            OnPropertyChanged();
        }
    }
    private bool _isDataLoaded;
    public bool IsDataLoaded
    {
        get => _isDataLoaded;
        set
        {
            _isDataLoaded = value;
            OnPropertyChanged();
        }
    }
    public bool IsProcessing
    {
        get => _isProcessing;
        set
        {
            _isProcessing = value;
            OnPropertyChanged();
        }
    }

    private ObservableCollection<ListModel> _systemstaffdesignation;
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
    private ObservableCollection<ListModel> _systemgender;
    public ObservableCollection<ListModel> Systemgender
    {
        get => _systemgender;
        set
        {
            _systemgender = value;
            OnPropertyChanged();
        }
    }


    private ListModel _selectedstaffgender;
    public ListModel Selectedstaffgender
    {
        get => _selectedstaffgender;
        set
        {
            _selectedstaffgender = value;
            // Ensure SystempropertyData is not null
            if (Systemgender != null)
            {
                // Safely convert the selected value to long and assign it to Countyid
                if (value != null && int.TryParse(value.Value?.ToString(), out int genderid))
                {
                    StaffData.Genderid = genderid;
                }
                else
                {
                    StaffData.Genderid = 0;
                }

                OnPropertyChanged(nameof(Selectedstaffgender));
                OnPropertyChanged(nameof(StaffData.Genderid));
            }
        }
    }
    private ObservableCollection<ListModel> _systemmaritalstatus;
    public ObservableCollection<ListModel> Systemmaritalstatus
    {
        get => _systemmaritalstatus;
        set
        {
            _systemmaritalstatus = value;
            OnPropertyChanged();
        }
    }
    private ListModel _selectedstaffmaritalstatus;
    public ListModel Selectedstaffmaritalstatus
    {
        get => _selectedstaffmaritalstatus;
        set
        {
            _selectedstaffmaritalstatus = value;
            // Ensure SystempropertyData is not null
            if (Systemmaritalstatus != null)
            {
                // Safely convert the selected value to long and assign it to Countyid
                if (value != null && int.TryParse(value.Value?.ToString(), out int maritalstatusid))
                {
                    StaffData.Maritalstatusid = maritalstatusid;
                }
                else
                {
                    StaffData.Maritalstatusid = 0;
                }

                OnPropertyChanged(nameof(Selectedstaffmaritalstatus));
                OnPropertyChanged(nameof(StaffData.Maritalstatusid));
            }
        }
    }
    private ObservableCollection<ListModel> _systemkinrelationship;
    public ObservableCollection<ListModel> Systemkinrelationship
    {
        get => _systemkinrelationship;
        set
        {
            _systemkinrelationship = value;
            OnPropertyChanged();
        }
    }
    private ListModel _selectedstaffkinrelationship;
    public ListModel Selectedstaffkinrelationship
    {
        get => _selectedstaffkinrelationship;
        set
        {
            _selectedstaffkinrelationship = value;
            // Ensure SystempropertyData is not null
            if (Systemkinrelationship != null)
            {
                // Safely convert the selected value to long and assign it to Countyid
                if (value != null && int.TryParse(value.Value?.ToString(), out int kinrelationshipid))
                {
                    StaffData.Kinrelationshipid = kinrelationshipid;
                }
                else
                {
                    StaffData.Kinrelationshipid = 0;
                }

                OnPropertyChanged(nameof(Selectedstaffkinrelationship));
                OnPropertyChanged(nameof(StaffData.Kinrelationshipid));
            }
        }
    }


    public ICommand TogglePasswordVisibilityCommand { get; }
    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }
    public ICommand ForgotPasswordCommand { get; }

    public ICommand SignUpCommand => new Command(async () => await OnSignUp());
    public ICommand ToggleConfirmPasswordVisibilityCommand { get; }


    public ICommand LoadCurrentUserCommand { get; }
    public ICommand UpdateCurrentUserDetailsCommand { get; }
    public ICommand SubmitCurrentUserDetailsCommand { get; }
    public LoginPageViewModel(BL bl)
    {
        _bl = bl;
        StaffData = new SystemStaff();
        IsPasswordHidden = true; // Default to hidden password
        TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);
        IsPasswordHidden = true; // Default to hidden password
        IsConfirmPasswordHidden = true; // Default to hidden password
        ToggleConfirmPasswordVisibilityCommand = new Command(ToggleConfirmPasswordVisibility);
        Systemstaffdesignation = new ObservableCollection<ListModel>
        {
            new ListModel { Value = "Collectioncenter", Text = "Collection Center" },
            new ListModel { Value = "Parcelcourier", Text = "Courier" },
            new ListModel { Value = "Parcelcustomer", Text = "Customer" },
        };
        LoginCommand = new Command(async () => await LoginAsync());
        RegisterCommand = new Command(OnRegister);
        ForgotPasswordCommand = new Command(OnForgotPassword);
        LoadCurrentUserCommand = new Command(async () => await LoadCurrentUserDataAsync());
        UpdateCurrentUserDetailsCommand = new Command(async () => await Updateuserdetailsasync());
        SubmitCurrentUserDetailsCommand = new Command(async () => await Submituserdetailsasync());
    }
    private void ToggleConfirmPasswordVisibility()
    {
        IsConfirmPasswordHidden = !IsConfirmPasswordHidden;
    }
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }


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

    private void TogglePasswordVisibility()
    {
        IsPasswordHidden = !IsPasswordHidden;
    }
    private async Task LoginAsync()
    {
        IsProcessing = true;
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

            var response = await _bl.ValidateSystemStaff(request.username, request.password);

            if (response.RespStatus == 200 || response.RespStatus == 0)
            {
                App.UserDetails = response;
                if (response.Usermodel.Loginstatus == (int)UserLoginStatus.VerifyAccount)
                {
                    var response1 = await _bl.Getsystemstaffdetaildatabyid(response.Usermodel.Userid);
                    if (response1 != null)
                    {
                        SystemStaffTenantData = response1.Data;
                    }
                    var accountVerificationPage = new ValidateStaffAccountPage(this);
                    await Shell.Current.Navigation.PushAsync(accountVerificationPage);
                }
                else if (response.Usermodel.Loginstatus == (int)UserLoginStatus.Ok)
                {
                    if (response.Usermodel.Updateprofile)
                    {
                        UserName = "";
                        Password = "";
                        await Shell.Current.GoToAsync(nameof(UpdateUserProfilePage));
                    }
                    else
                    {
                        string userDetailStr = JsonConvert.SerializeObject(response);
                        Preferences.Set(nameof(App.UserDetails), userDetailStr);
                        await AppConstant.AddFlyoutMenusDetails();
                    }
                }
                else
                {
                    if (DeviceInfo.Platform == DevicePlatform.WinUI)
                    {
                        AppShell.Current.Dispatcher.Dispatch(async () =>
                        {
                            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                        });
                    }
                    else
                    {
                        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    }
                }
            }
            else if (response.RespStatus == 1)
            {
                await Shell.Current.DisplayAlert("Warning", response.RespMessage, "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", response.RespMessage, "OK");
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

    private async Task OnSignUp()
    {
        IsProcessing = true;

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
            var response = await _bl.Registersystemstaffdata(request);
            if (response.RespStatus == 0 || response.RespStatus == 200)
            {
                FirstName = "";
                LastName = "";
                EmailAddress = "";
                PhoneNumber = "";
                StaffDesignation = "";
                Password = "";
                ConfirmPassword = "";
                Accepttermsandcondition = false;
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

    private async Task LoadCurrentUserDataAsync()
    {
        IsProcessing = true;
        IsDataLoaded = false;
        try
        {
            var response = await _bl.Getsystemstaffprofiledatabyid(App.UserDetails.Usermodel.Userid);
            if (response != null)
            {
                var SystemgenderResponse = await _bl.GetListModel(ListModelType.Systemgender);
                if (SystemgenderResponse != null)
                {
                    Systemgender = new ObservableCollection<ListModel>(SystemgenderResponse);
                }
                var SystemmaritalstatusResponse = await _bl.GetListModel(ListModelType.Systemmaritalstatus);
                if (SystemmaritalstatusResponse != null)
                {
                    Systemmaritalstatus = new ObservableCollection<ListModel>(SystemmaritalstatusResponse);
                }
                var SystemkinrelationshipResponse = await _bl.GetListModel(ListModelType.Systemkinrelationship);
                if (SystemkinrelationshipResponse != null)
                {
                    Systemkinrelationship = new ObservableCollection<ListModel>(SystemkinrelationshipResponse);
                }
                StaffData = response.Data;
                if (StaffData.Genderid > 0)
                {
                    Selectedstaffgender = Systemgender.FirstOrDefault(x => x.Value == _staffData.Genderid.ToString());
                }
                if (StaffData.Maritalstatusid > 0)
                {
                    Selectedstaffmaritalstatus = Systemmaritalstatus.FirstOrDefault(x => x.Value == _staffData.Maritalstatusid.ToString());
                }
                if (StaffData.Kinrelationshipid > 0)
                {
                    Selectedstaffkinrelationship = Systemkinrelationship.FirstOrDefault(x => x.Value == _staffData.Kinrelationshipid.ToString());
                }
            }
            IsDataLoaded = true;
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsProcessing = false;
        }
    }
    private async Task Updateuserdetailsasync()
    {
        IsProcessing = true;

        if (!IsValidInput())
        {
            IsProcessing = false;
            return;
        }
        if (StaffData == null)
        {
            IsProcessing = false;
            return;
        }

        try
        {
            IsProcessing = true;
            StaffData.Updateprofile = false;
            StaffData.Modifiedby = App.UserDetails.Usermodel.Userid;
            StaffData.Datemodified = DateTime.Now;
            // Call your registration service here
            var response = await _bl.Registersystemstaffdata(StaffData);
            if (response.RespStatus == 200 || response.RespStatus == 0)
            {
                string userDetailStr = JsonConvert.SerializeObject(App.UserDetails);
                Preferences.Set(nameof(App.UserDetails), userDetailStr);
                await AppConstant.AddFlyoutMenusDetails();
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


    private async Task Submituserdetailsasync()
    {
        IsProcessing = true;

        if (!IsValidInput())
        {
            IsProcessing = false;
            return;
        }
        if (StaffData == null)
        {
            IsProcessing = false;
            return;
        }

        try
        {
            IsProcessing = true;
            StaffData.Updateprofile = false;
            StaffData.Modifiedby = App.UserDetails.Usermodel.Userid;
            StaffData.Datemodified = DateTime.Now;
            // Call your registration service here
            var response = await _bl.Registersystemstaffdata(StaffData);
            if (response.RespStatus == 200 || response.RespStatus == 0)
            {
                await Shell.Current.DisplayAlert("Success", response.RespMessage, "OK");
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

        if (string.IsNullOrWhiteSpace(UserName))
        {
            SystemStaffUserNameError = "Email Address is required.";
            isValid = false;
        }
        else if (!IsValidEmail(UserName))
        {
            SystemStaffUserNameError = "Invalid email address format.";
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
        IsProcessing = true;
        SystemStaff systemStaff = null;
        await Shell.Current.GoToAsync(nameof(RegisterPage));
        IsProcessing = false;
    }
    private async void OnForgotPassword()
    {
        await Shell.Current.GoToAsync(nameof(ForgotPasswordPage));
    }
}
