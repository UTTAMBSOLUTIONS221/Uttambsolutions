using DBL.Models;
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
        private PropertyHouseRoomTenantData _tenantData;

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

        public PropertyHouseRoomTenantData TenantData
        {
            get => _tenantData;
            set
            {
                _tenantData = value;
                OnPropertyChanged(nameof(TenantData));
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
            LoadItemsCommand = new Command<PropertyHouseDetails>(async (propertyRoom) => await PropertyHousesTenantDetails(propertyRoom.Systempropertyhouseroomid));
        }

        private async Task PropertyHousesTenantDetails(long Tenantid)
        {
            IsProcessing = true;
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousetenantdatabytenantid/" + Tenantid, HttpMethod.Get, null);
            if (response != null)
            {
                TenantData = JsonConvert.DeserializeObject<PropertyHouseRoomTenantData>(response.Data.ToString());
                TenantData.Tenantroomdata.Expectedvacatingdate = DateTime.Now.AddMonths(TenantData.Tenantroomdata.Vacatingperioddays);
                Isvisible = TenantData.Tenantroomdata.Occupationalstatus == "Occupant";
            }
            //var modalPage = new HousesRoomDetailModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
