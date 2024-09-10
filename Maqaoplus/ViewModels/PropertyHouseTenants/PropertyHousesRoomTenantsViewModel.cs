using DBL.Entities;
using DBL.Models;
using Maqaoplus.Views.PropertyHouseTenants;
using Maqaoplus.Views.PropertyHouseTenants.Modal;
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
        private readonly Services.ServiceProvider _serviceProvider;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        public ObservableCollection<PropertyHouseTenant> Items { get; }
        private PropertyHouseRoomTenantData _tenantData;
        private SystemStaff _staffData;
        public ICommand LoadItemsCommand { get; }
        public ICommand LoadAgentItemsCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand AddPropertyHouseAgentTenantCommand { get; }

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
        public SystemStaff StaffData
        {
            get => _staffData;
            set
            {
                _staffData = value;
                OnPropertyChanged();
            }
        }
        // Constructor with ServiceProvider parameter
        public PropertyHousesRoomTenantsViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Items = new ObservableCollection<PropertyHouseTenant>();
            TenantData = new PropertyHouseRoomTenantData();
            StaffData = new SystemStaff();
            LoadItemsCommand = new Command(async () => await LoadItems());
            LoadAgentItemsCommand = new Command(async () => await LoadAgentItems());
            ViewDetailsCommand = new Command<PropertyHouseTenant>(async (propertyhousetenant) => await ViewDetails(propertyhousetenant.Systempropertyhousetenantid));
            AddPropertyHouseAgentTenantCommand = new Command<SystemStaff>(async (propertytenant) => await AddPropertyHouseAgentTenant(propertytenant.Userid));
        }
        private async Task LoadItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhouseroomtenantsdata/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null && response.Data is List<dynamic> items)
                {
                    Items.Clear();
                    foreach (var item in items)
                    {
                        var product = item.ToObject<PropertyHouseTenant>();
                        Items.Add(product);
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
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystemagentpropertyhouseroomtenantsdata/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null && response.Data is List<dynamic> items)
                {
                    Items.Clear();
                    foreach (var item in items)
                    {
                        var product = item.ToObject<PropertyHouseTenant>();
                        Items.Add(product);
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
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousetenantdatabytenantid/" + Tenantid, HttpMethod.Get, null);
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
        private async Task AddPropertyHouseAgentTenant(long Tenantid)
        {
            IsProcessing = true;
            if (Tenantid > 0)
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousetenantdatabytenantid/" + Tenantid, HttpMethod.Get, null);
                if (response != null)
                {
                    StaffData = JsonConvert.DeserializeObject<SystemStaff>(response.Data.ToString());
                }
            }
            var detailPage = new AddAgentPropertyHouseTenantPage(this);
            await Application.Current.MainPage.Navigation.PushAsync(detailPage);
            IsProcessing = false;
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
