using DBL;
using DBL.Entities;
using DBL.Enum;
using Newtonsoft.Json;
using Parceldrop.Views.Startup;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Parceldrop.ViewModels.Startup;
public class LoginPageViewModel : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    private readonly BL _bl;
    private bool _isPasswordHidden;
    private string _passwordIconSource;
    public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";

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



    public LoginPageViewModel(BL bl)
    {
        _bl = bl;
        IsPasswordHidden = true; // Default to hidden password
        TogglePasswordVisibilityCommand = new Command(TogglePasswordVisibility);
        LoginCommand = new Command(async () => await LoginAsync(), () => !IsProcessing);
        //RegisterCommand = new Command(OnRegister);
        //ForgotPasswordCommand = new Command(OnForgotPassword);
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
    public ICommand TogglePasswordVisibilityCommand { get; }
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
                    var encodedStaffId = Uri.EscapeDataString(response.Usermodel.Userid.ToString());
                    await Shell.Current.GoToAsync($"ValidateStaffAccountPage?UserId={encodedStaffId}");
                }
                else if (response.Usermodel.Loginstatus != 0)
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
                else
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
                        // Example additional logic after successful login
                        //await AppConstant.AddFlyoutMenusDetails();
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


    //private async void OnRegister()
    //{
    //    SystemStaff systemStaff = null;
    //    await Shell.Current.GoToAsync(nameof(RegisterPage));
    //}
    //private async void OnForgotPassword()
    //{
    //    await Shell.Current.GoToAsync(nameof(ForgotPasswordPage));
    //}
}
