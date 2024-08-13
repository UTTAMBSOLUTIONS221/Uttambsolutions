using DBL.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouse
{
    public class PropertyHousesRoomTenantsViewModel : BaseViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public ObservableCollection<PropertyHouseTenant> Items { get; }
        public ICommand LoadItemsCommand { get; }
        public ICommand ViewDetailsCommand { get; }

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

        // Parameterless constructor for XAML support
        public PropertyHousesRoomTenantsViewModel()
        {
            Items = new ObservableCollection<PropertyHouseTenant>();
            LoadItemsCommand = new Command(async () => await LoadItems());
            ViewDetailsCommand = new Command<PropertyHouseTenant>(async (propertyhousetenant) => await ViewDetails(propertyhousetenant.Systempropertyhousetenantid));
        }

        // Constructor with ServiceProvider parameter
        public PropertyHousesRoomTenantsViewModel(Services.ServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
        }
        private async Task LoadItems()
        {
            IsLoading = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhouseroomtenantsdata/" + App.UserDetails.Usermodel.Userid + "/0", HttpMethod.Get, null);
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
                IsLoading = false;
            }
        }
        private async Task ViewDetails(long propertyhousetenantid)
        {
            IsLoading = true;
            IsDataLoaded = false;
            try
            {
                var encodedPropertyhousetenantid = Uri.EscapeDataString(propertyhousetenantid.ToString());
                await Shell.Current.GoToAsync($"PropertyHousesTenantDetailPage?Propertyhousetenantid={encodedPropertyhousetenantid}");
                IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Navigation Error", ex.Message, "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
