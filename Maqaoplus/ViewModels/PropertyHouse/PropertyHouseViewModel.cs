using DBL.Entities;
using Maqaoplus.Views.PropertyHouse;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouse
{
    public class PropertyHouseViewModel : BaseViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public ObservableCollection<Systemproperty> Items { get; }

        public ICommand AddPropertyHouseCommand { get; }
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
        public PropertyHouseViewModel()
        {
            Items = new ObservableCollection<Systemproperty>();
            AddPropertyHouseCommand = new Command(AddPropertyHouseAsync);
            LoadItemsCommand = new Command(async () => await LoadItems());
            ViewDetailsCommand = new Command<Systemproperty>(async (property) => await ViewDetails(property.Propertyhouseid));
        }

        // Constructor with ServiceProvider parameter
        public PropertyHouseViewModel(Services.ServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
        }
        private async void AddPropertyHouseAsync()
        {
            IsLoading = true;
            await Shell.Current.GoToAsync(nameof(AddPropertyHousePage));
            IsLoading = false;
        }
        private async Task LoadItems()
        {
            IsLoading = true;
            IsDataLoaded = false;

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
        private async Task ViewDetails(long propertyId)
        {
            IsLoading = true;
            IsDataLoaded = false;
            try
            {
                var encodedPropertyId = Uri.EscapeDataString(propertyId.ToString());
                System.Diagnostics.Debug.WriteLine($"Navigating to PropertyHousesDetailPage with PropertyId={encodedPropertyId}");
                await Shell.Current.GoToAsync($"PropertyHousesDetailPage?PropertyId={encodedPropertyId}");
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
