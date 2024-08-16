using DBL.Models;
using Maqaoplus.Views.PropertyHouseTenants.Modal;
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
        public ICommand NeedtoVacateCommand { get; }
        public ICommand OnCancelClickedCommand { get; }

        public PropertyHouseRoomTenantData TenantData
        {
            get => _tenantData;
            set
            {
                _tenantData = value;
                OnPropertyChanged();
            }
        }
        private bool _isProcessing;
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged();
            }
        }
        private bool _isVacatingProcessing;
        public bool IsVacatingProcessing
        {
            get => _isVacatingProcessing;
            set
            {
                _isVacatingProcessing = value;
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

        public Propertyhousetenantviewmodel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadItemsCommand = new Command(async () => await LoadItems());
            NeedtoVacateCommand = new Command(async () => await NeedtoVacatethisHouseAsync());
            OnCancelClickedCommand = new Command(OnCancelClicked);
        }
        // Error properties
        private DateTime _expectedVacatingDate;
        public DateTime ExpectedVacatingDate
        {
            get => _expectedVacatingDate;
            set
            {
                _expectedVacatingDate = value;
                OnPropertyChanged();
            }
        }
        private async Task LoadItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousetenantdatabytenantid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null)
                {
                    TenantData = JsonConvert.DeserializeObject<PropertyHouseRoomTenantData>(response.Data.ToString());
                    ExpectedVacatingDate = DateTime.Now.AddMonths(TenantData.Tenantroomdata.Vacatingperioddays);
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



        private async Task NeedtoVacatethisHouseAsync()
        {
            IsVacatingProcessing = true;
            var modalPage = new TenantVacationNoticeModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsVacatingProcessing = false;
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
