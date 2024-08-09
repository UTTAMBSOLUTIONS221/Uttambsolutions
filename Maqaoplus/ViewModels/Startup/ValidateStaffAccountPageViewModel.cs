using DBL.Models;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.Startup
{
    public class ValidateStaffAccountPageViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        private PropertyHouseRoomTenantData _tenantData;
        private long _userId;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoadCurrentUserCommand { get; }
        public void SetUserId(long userId)
        {
            _userId = userId;
            LoadCurrentUserCommand.Execute(null);
        }
        public PropertyHouseRoomTenantData StaffData
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

        public ValidateStaffAccountPageViewModel()
        {
            LoadCurrentUserCommand = new Command(async () => await LoadCurrentUserData());
        }

        // Constructor with parameter
        public ValidateStaffAccountPageViewModel(Services.ServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
        }

        private async Task LoadCurrentUserData()
        {
            IsLoading = true;
            IsDataLoaded = false;

            try
            {
                //var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousetenantdatabytenantid/" + 2, HttpMethod.Get, null);
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousetenantdatabytenantid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null)
                {
                    StaffData = JsonConvert.DeserializeObject<PropertyHouseRoomTenantData>(response.Data.ToString());
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