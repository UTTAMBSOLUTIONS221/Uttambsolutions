﻿using DBL.Models;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouseTenantAgreement
{
    public class PropertyHouseTenantAgreementViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public event PropertyChangedEventHandler PropertyChanged;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        private TenantAgreementDetailData _tenantAgreementDetailData;
        public ICommand ViewPropertyRoomAgreementCommand { get; }

        private bool _isProcessing;
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged(nameof(IsProcessing));
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
        public TenantAgreementDetailData TenantAgreementDetailData
        {
            get => _tenantAgreementDetailData;
            set
            {
                _tenantAgreementDetailData = value;
                OnPropertyChanged(nameof(TenantAgreementDetailData));
                OnPropertyChanged(nameof(IsSignatureDrawingVisible));
                OnPropertyChanged(nameof(IsSignatureImageVisible));
                OnPropertyChanged(nameof(IsSignatureAvailable));
            }
        }

        public bool IsSignatureDrawingVisible => string.IsNullOrEmpty(TenantAgreementDetailData?.TenantSignatureimageurl);
        public bool IsSignatureImageVisible => !string.IsNullOrEmpty(TenantAgreementDetailData?.TenantSignatureimageurl);
        public bool IsSignatureAvailable => !string.IsNullOrEmpty(TenantAgreementDetailData?.TenantSignatureimageurl);
        // Parameterless constructor for XAML support
        public PropertyHouseTenantAgreementViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            ViewPropertyRoomAgreementCommand = new Command(async () => await ViewPropertyRoomAgreementDetails());
        }

        private async Task ViewPropertyRoomAgreementDetails()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhouseroomagreementdetaildatabytenantid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null)
                {
                    TenantAgreementDetailData = JsonConvert.DeserializeObject<TenantAgreementDetailData>(response.Data.ToString());
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

        public async Task AgreeToPropertyHouseRoomAgreementasync(string imageUrl)
        {
            IsProcessing = true;

            await Task.Delay(500);
            if (TenantAgreementDetailData == null)
            {
                IsProcessing = false;
                return;
            }
            TenantAgreementDetailData.Propertyhouseowner = App.UserDetails.Usermodel.Userid;
            TenantAgreementDetailData.Signatureimageurl = imageUrl;
            TenantAgreementDetailData.Ownerortenant = "Tenant";
            TenantAgreementDetailData.Agreementname = TenantAgreementDetailData.Tenantfullname + " Room " + TenantAgreementDetailData.Propertyhousename + " " + TenantAgreementDetailData.Systempropertyhousesizename + " " + TenantAgreementDetailData.Systempropertyhousesizename + " Tenant Agreement";
            TenantAgreementDetailData.Datecreated = DateTime.UtcNow;
            try
            {
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseagreementdata", TenantAgreementDetailData);
                if (response.RespStatus == 200 || response.RespStatus == 0)
                {
                    Application.Current.MainPage.Navigation.PopModalAsync();
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
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsProcessing = false;
            }
        }


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
