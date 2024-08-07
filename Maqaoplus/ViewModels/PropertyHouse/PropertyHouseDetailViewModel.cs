using DBL.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouse
{
    public class PropertyHouseDetailViewModel : BaseViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        private long _propertyId;

        public ObservableCollection<PropertyHouseDetails> Rooms { get; }
        public ICommand LoadRoomsCommand { get; }

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

        // Parameterless constructor
        public PropertyHouseDetailViewModel()
        {
            Rooms = new ObservableCollection<PropertyHouseDetails>();
            LoadRoomsCommand = new Command(async () => await LoadRooms());
        }

        // Constructor with parameter
        public PropertyHouseDetailViewModel(Services.ServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
        }

        public void SetPropertyId(long propertyId)
        {
            _propertyId = propertyId;
            LoadRoomsCommand.Execute(null);
        }

        private async Task LoadRooms()
        {
            IsLoading = true;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousedetaildatabypropertyidandownerid/" + _propertyId + "/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null && response.Data is List<dynamic> items)
                {
                    Rooms.Clear();
                    foreach (var item in items)
                    {
                        var room = item.ToObject<PropertyHouseDetails>();
                        Rooms.Add(room);
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
    }

}