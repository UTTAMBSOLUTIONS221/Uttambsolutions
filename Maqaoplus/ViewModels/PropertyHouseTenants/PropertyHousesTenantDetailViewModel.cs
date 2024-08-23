﻿using DBL.Models;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouseTenants
{
    public class PropertyHousesTenantDetailViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        private long _propertyHousesTenantIdNumber;
        private Systemtenantdetails _tenantDetailData;

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
                OnPropertyChanged(nameof(IsDataLoaded));
            }
        }
        private bool _isValid;
        public bool IsValid
        {
            get => _isValid;
            private set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }
        public ICommand LoadItemsCommand { get; }


        public Systemtenantdetails TenantDetailData
        {
            get => _tenantDetailData;
            set
            {
                _tenantDetailData = value;
                OnPropertyChanged(nameof(TenantDetailData));
            }
        }

        public void SetPropertyHousesTenantIdNumber(long propertyHousesTenantIdNumber)
        {
            _propertyHousesTenantIdNumber = propertyHousesTenantIdNumber;
            LoadItemsCommand.Execute(null);
        }
        public PropertyHousesTenantDetailViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadItemsCommand = new Command(async () => await PropertyHousesTenantDetails());
        }

        private async Task PropertyHousesTenantDetails()
        {
            IsProcessing = true;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>($"/api/Account/Getsystemstaffdetaildatabyidnumber/" + _propertyHousesTenantIdNumber, HttpMethod.Get, null);

                if (response != null)
                {
                    TenantDetailData = JsonConvert.DeserializeObject<Systemtenantdetails>(response.Data.ToString());
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
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    }
}
