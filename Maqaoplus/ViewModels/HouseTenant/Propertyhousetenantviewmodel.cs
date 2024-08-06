using DBL.Models;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maqaoplus.ViewModels.HouseTenant
{
    public class Propertyhousetenantviewmodel : INotifyPropertyChanged
    {
        private readonly HttpClient _httpClient;
        private PropertyHouseRoomTenantData _tenantData;

        public event PropertyChangedEventHandler PropertyChanged;

        public PropertyHouseRoomTenantData TenantData
        {
            get => _tenantData;
            set
            {
                _tenantData = value;
                OnPropertyChanged();
            }
        }

        public Propertyhousetenantviewmodel()
        {
            _httpClient = new HttpClient();
            LoadData();
        }

        private async void LoadData()
        {
            var response = await _httpClient.GetStringAsync("https://yourapiurl.com/api/tenantdata");
            TenantData = JsonConvert.DeserializeObject<PropertyHouseRoomTenantData>(response);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
