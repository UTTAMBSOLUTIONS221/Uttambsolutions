using DBL.Entities;
using DBL.Enum;
using DBL.Models;
using Maqaoplus.Views;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
namespace Maqaoplus.ViewModels.Startup
{
    public class ValidateStaffAccountPageViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";

        private StaffDetailData _systemStaffTenantData;
        private SystemStaff _tenantData;
        private string _paymentReferenceCode;
        private long _userid;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoadCurrentUserCommand { get; }
        public ICommand CheckUserLoginStatusCommand { get; }
        public ICommand OnCancelClickedCommand { get; }
        public ICommand OnOkClickedCommand { get; }
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
                if (_tenantData != null)
                {
                    Userid = _tenantData.Userid;
                }
            }
        }
        public string PaymentReferenceCode
        {
            get => _paymentReferenceCode;
            set
            {
                _paymentReferenceCode = value;
                OnPropertyChanged();
                ((Command)OnOkClickedCommand).ChangeCanExecute();
            }
        }
        public long Userid
        {
            get => _userid;
            set
            {
                _userid = value;
                OnPropertyChanged();
                ((Command)OnOkClickedCommand).ChangeCanExecute();
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

        private string _paymentReferenceCodeError;
        public string PaymentReferenceCodeError
        {
            get => _paymentReferenceCodeError;
            set
            {
                _paymentReferenceCodeError = value;
                OnPropertyChanged();
            }
        }
        public ValidateStaffAccountPageViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadCurrentUserCommand = new Command(async () => await LoadCurrentyStaffTenantData());
            CheckUserLoginStatusCommand = new Command(async () => await CheckUserLoginStatusAsync());
            OnCancelClickedCommand = new Command(OnCancelClicked);
            OnOkClickedCommand = new Command(async () => await OnOkClickedAsync());

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
                        var modalPage = new ConfirmPaymentDetailModalPage(this);
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
        private async Task OnOkClickedAsync()
        {
            IsProcessing = true;

            if (!IsValidInput())
            {
                IsProcessing = false;
                return;
            }
            var request = new PaymentConfirmation
            {
                Userid = Userid,
                PaymentReferenceCode = PaymentReferenceCode
            };
            //Validate Family Bank 
            var response1 = await _serviceProvider.CallAuthWebApi<object>("/api/Account/Getsystemstaffdetaildatabyid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
            if (response1 != null)
            {

            }
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void OnCancelClicked()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private bool IsValidInput()
        {
            bool isValid = true;
            if (string.IsNullOrWhiteSpace(PaymentReferenceCode))
            {
                PaymentReferenceCodeError = "Payment Reference Code is required.";
                isValid = false;
            }
            else
            {
                PaymentReferenceCodeError = null;
            }
            return isValid;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}