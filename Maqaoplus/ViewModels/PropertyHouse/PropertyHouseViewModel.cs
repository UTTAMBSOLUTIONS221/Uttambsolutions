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

        // Parameterless constructor for XAML support
        public PropertyHouseViewModel()
        {
            Items = new ObservableCollection<Systemproperty>();
            LoadItemsCommand = new Command(async () => await LoadItems());
            ViewDetailsCommand = new Command<Systemproperty>(async (item) => await ViewDetails(item));
        }

        // Constructor with ServiceProvider parameter
        public PropertyHouseViewModel(Services.ServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
        }

        private async Task LoadItems()
        {
            IsBusy = true;

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
                IsBusy = false;
            }
        }
        private async Task ViewDetails(Systemproperty item)
        {
            try
            {
                var jsonProperty = JsonConvert.SerializeObject(item);
                var encodedProperty = Uri.EscapeDataString(jsonProperty);
                await Shell.Current.GoToAsync($"PropertyHousesDetailPage?Property={encodedProperty}");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Navigation Error", ex.Message, "OK");
            }
        }
    }
}
