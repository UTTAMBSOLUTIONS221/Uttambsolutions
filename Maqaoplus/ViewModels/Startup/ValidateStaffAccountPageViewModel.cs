using DBL.Entities;
using DBL.Enum;
using DBL.Models;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.Startup
{
    public class ValidateStaffAccountPageViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        private StaffDetailData _systemStaffTenantData;
        private SystemStaff _tenantData;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoadCurrentUserCommand { get; }
        public ICommand CheckUserLoginStatusCommand { get; }
        private bool _isProcessing;
        public StaffDetailData SystemStaffTenantData
        {
            get => _systemStaffTenantData;
            set
            {
                _systemStaffTenantData = value;
                OnPropertyChanged();
            }
        }
        public SystemStaff StaffData
        {
            get => _tenantData;
            set
            {
                _tenantData = value;
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
                ((Command)CheckUserLoginStatusCommand).ChangeCanExecute();
            }
        }
        public ValidateStaffAccountPageViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadCurrentUserCommand = new Command(async () => await LoadCurrentyStaffTenantData());
            CheckUserLoginStatusCommand = new Command(async () => await CheckUserLoginStatusAsync(), () => !IsProcessing);
        }

        private async Task LoadCurrentyStaffTenantData()
        {
            IsProcessing = true;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/Account/Getsystemstaffdetaildatabyid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null)
                {
                    SystemStaffTenantData = JsonConvert.DeserializeObject<StaffDetailData>(response.Data.ToString());
                }
                IsProcessing = true;
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

        private async Task CheckUserLoginStatusAsync()
        {
            IsProcessing = true;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/Account/Getsystemstaffdetaildatabyid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null)
                {
                    StaffData = JsonConvert.DeserializeObject<SystemStaff>(response.Data.ToString());
                    if (StaffData.Loginstatus == (int)UserLoginStatus.Ok)
                    {
                        await Shell.Current.GoToAsync("//LoginPage");
                    }
                    else
                    {
                        var modalPage = new ConfirmPaymentDetailModalPage(
                        StaffData,
                        new Command(OnOkClicked),
                        new Command(OnCancelClicked)
                    );
                        await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
                    }
                }
                IsProcessing = true;
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
        private void OnOkClicked()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void OnCancelClicked()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}