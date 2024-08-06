using DBL.Models;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.HouseTenant
{
    public class Propertyhousetenantviewmodel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        private PropertyHouseRoomTenantData _tenantData;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoadItemsCommand { get; }

        public PropertyHouseRoomTenantData TenantData
        {
            get => _tenantData;
            set
            {
                _tenantData = value;
                OnPropertyChanged();
            }
        }

        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
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
        public Propertyhousetenantviewmodel()
        {
            LoadItemsCommand = new Command(async () => await LoadItems());
        }
        private async Task LoadItems()
        {
            IsLoading = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousetenantdatabytenantid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null)
                {
                    TenantData = JsonConvert.DeserializeObject<PropertyHouseRoomTenantData>(response.Data.ToString());
                }
                IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
