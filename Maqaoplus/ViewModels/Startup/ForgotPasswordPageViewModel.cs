using DBL.Entities;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
namespace Maqaoplus.ViewModels.Startup
{
    public class ForgotPasswordPageViewModel : INotifyPropertyChanged
    {
        private string _emailAddress;
        private bool _isProcessing;

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Services.ServiceProvider _serviceProvider;

        public ForgotPasswordPageViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
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
        private async Task ForgotPasswordAsync()
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

                var request = new Forgotpassword
                {
                    Emailaddress = EmailAddress
                };

                // Call your registration service here
                var response = await _serviceProvider.CallUnAuthWebApi("/api/Account/Forgotstaffpassword", HttpMethod.Post, request);
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
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(EmailAddress))
            {
                SystemStaffEmailAddressError = "Email Address is required.";
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
