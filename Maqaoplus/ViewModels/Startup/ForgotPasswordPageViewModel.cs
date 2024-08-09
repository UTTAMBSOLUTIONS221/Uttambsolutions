using DBL.Entities;
using Maqaoplus.Views.Startup;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
namespace Maqaoplus.ViewModels.Startup
{
    public class ForgotPasswordPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Services.ServiceProvider _serviceProvider;

        public ForgotPasswordPageViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            ForgotPasswordCommand = new Command(async () => await ForgotPasswordAsync(), () => !IsProcessing);
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
                ((Command)ForgotPasswordCommand).ChangeCanExecute();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
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

        public ICommand ForgotPasswordCommand { get; }

        private async Task ForgotPasswordAsync()
        {
            if (IsProcessing || string.IsNullOrWhiteSpace(UserName))
                return;

            try
            {
                IsProcessing = true;

                var request = new Forgotpassword
                {
                    Emailaddress = UserName,
                };
                var response = await _serviceProvider.CallUnAuthWebApi("/api/Account/Registerstaff", HttpMethod.Post, request);
                if (response.StatusCode == 200)
                {
                    await Shell.Current.GoToAsync(nameof(LoginPage));
                }
                else
                {
                    await Shell.Current.DisplayAlert("Uttamb Solutions", response.StatusMessage, "OK");
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
    }
}
