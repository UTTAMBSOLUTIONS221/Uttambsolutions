﻿using DBL;
using DBL.Models;
using Maqaoplus.Views.PropertyHouseTenants;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouseTenants
{
    public class PropertyHousesRoomTenantsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private readonly BL _bl;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        public ObservableCollection<PropertyHouseTenant> Items { get; }
        private PropertyHouseRoomTenantData _tenantData;
        public ICommand LoadItemsCommand { get; }
        public ICommand LoadAgentItemsCommand { get; }
        public ICommand ViewDetailsCommand { get; }

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

        private bool _isvisible;

        public bool Isvisible
        {
            get => _isvisible;
            set
            {
                _isvisible = value;
                OnPropertyChanged(nameof(Isvisible));
            }
        }

        public PropertyHouseRoomTenantData TenantData
        {
            get => _tenantData;
            set
            {
                _tenantData = value;
                OnPropertyChanged(nameof(TenantData));
            }
        }
        // Constructor with ServiceProvider parameter
        public PropertyHousesRoomTenantsViewModel(BL bl)
        {
            _bl = bl;
            Items = new ObservableCollection<PropertyHouseTenant>();
            TenantData = new PropertyHouseRoomTenantData();
            LoadItemsCommand = new Command(async () => await LoadItems());
            LoadAgentItemsCommand = new Command(async () => await LoadAgentItems());
            ViewDetailsCommand = new Command<PropertyHouseTenant>(async (propertyhousetenant) => await ViewDetails(propertyhousetenant.Systempropertyhousetenantid));
        }
        private async Task LoadItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _bl.Getsystempropertyhouseroomtenantsdata(App.UserDetails.Usermodel.Userid);
                if (response != null && response.Data != null)
                {
                    Items.Clear();
                    foreach (var item in response.Data)
                    {
                        Items.Add(item);
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
        private async Task LoadAgentItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _bl.Getsystemagentpropertyhouseroomtenantsdata(App.UserDetails.Usermodel.Userid);
                if (response != null && response.Data != null)
                {
                    Items.Clear();
                    foreach (var item in response.Data)
                    {
                        Items.Add(item);
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
        private async Task ViewDetails(long Tenantid)
        {
            IsProcessing = true;
            IsDataLoaded = false;
            var response = await _bl.Getsystempropertyhousetenantdatabytenantid(Tenantid);
            if (response != null)
            {
                TenantData = JsonConvert.DeserializeObject<PropertyHouseRoomTenantData>(response.Data.ToString());
                TenantData.Tenantroomdata.Expectedvacatingdate = DateTime.Now.AddMonths(TenantData.Tenantroomdata.Vacatingperioddays);
                Isvisible = TenantData.Tenantroomdata.Occupationalstatus == "Vacating";
                var detailPage = new PropertyHousesTenantDetailPage(this);

                await Application.Current.MainPage.Navigation.PushAsync(detailPage);
                IsDataLoaded = true;
                IsProcessing = false;
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
