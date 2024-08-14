﻿using DBL.Entities;
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
        private StaffDetailData _systemStaffTenantData;
        private SystemStaff _tenantData;
        private string _paymentReferenceCode;
        private long _userid;

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
        public string PaymentReferenceCode
        {
            get => _paymentReferenceCode;
            set
            {
                _paymentReferenceCode = value;
                OnPropertyChanged();
            }
        }
        public long Userid
        {
            get => _userid;
            set
            {
                _userid = value;
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
        private async void OnOkClicked()
        {
            IsProcessing = true;
            await Task.Delay(500);
            if (string.IsNullOrWhiteSpace(PaymentReferenceCode))
            {
                IsProcessing = false;
                await Application.Current.MainPage.DisplayAlert("Warning", "Payment Reference Code is required", "OK");
                return;
            }
            var request = new PaymentConfirmation
            {
                Userid = Userid,
                PaymentReferenceCode = PaymentReferenceCode
            };
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