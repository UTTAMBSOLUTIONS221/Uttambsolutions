using DBL.Entities;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouse
{
    public class PropertyHouseViewModel : BaseViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public ObservableCollection<Systemproperty> Items { get; }

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

        // Parameterless constructor for XAML support
        public PropertyHouseViewModel()
        {
            Items = new ObservableCollection<Systemproperty>();
            LoadItemsCommand = new Command(async () => await LoadItems());
            ViewDetailsCommand = new Command<int>(async (propertyId) => await ViewDetails(propertyId));
        }

        // Constructor with ServiceProvider parameter
        public PropertyHouseViewModel(Services.ServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
        }

        private async Task LoadItems()
        {
            IsLoading = true;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousedatabyowner/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null && response.Data is List<dynamic> items)
                {
                    Items.Clear();
                    foreach (var item in items)
                    {
                        var product = item.ToObject<Systemproperty>(); // Assuming item is of type JObject
                        Items.Add(product);
                    }
                }
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

        private async Task ViewDetails(int propertyId)
        {
            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>($"/api/PropertyHouse/GetPropertyDetails/{propertyId}", HttpMethod.Get, null);
                var propertyDetails = response?.Data; // Replace with actual deserialization

                if (propertyDetails != null)
                {
                    var jsonProperty = JsonConvert.SerializeObject(propertyDetails);
                    var encodedProperty = Uri.EscapeDataString(jsonProperty);
                    await Shell.Current.GoToAsync($"PropertyHousesDetailPage?Property={encodedProperty}");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Property details not found.", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Navigation Error", ex.Message, "OK");
            }
        }
    }
}
