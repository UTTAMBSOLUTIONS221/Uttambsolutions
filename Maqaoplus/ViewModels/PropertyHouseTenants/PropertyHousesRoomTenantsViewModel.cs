using DBL.Models;
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
        public ObservableCollection<PropertyHouseTenant> Items { get; }
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        public ICommand LoadItemsCommand { get; }
        public ICommand ViewDetailsCommand { get; }

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

        // Constructor with ServiceProvider parameter
        public PropertyHousesRoomTenantsViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Items = new ObservableCollection<PropertyHouseTenant>();
            LoadItemsCommand = new Command(async () => await LoadItems());
            ViewDetailsCommand = new Command<PropertyHouseTenant>(async (propertyhousetenant) => await ViewDetails(propertyhousetenant.Systempropertyhousetenantid));
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
        private async Task ViewDetails(long propertyhousetenantidnumber)
        {
            IsProcessing = true;
            IsDataLoaded = false;
            try
            {
                var encodedPropertyhousetenantidnumber = Uri.EscapeDataString(propertyhousetenantidnumber.ToString());
                await Shell.Current.GoToAsync($"PropertyHousesTenantDetailPage?Propertyhousetenantidnumber={encodedPropertyhousetenantidnumber}");
                IsDataLoaded = true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Navigation Error", ex.Message, "OK");
            }
            finally
            {
                IsProcessing = false;
            }
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
