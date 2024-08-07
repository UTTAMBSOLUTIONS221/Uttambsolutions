using DBL.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouse
{
    public class PropertyHouseRoomDetailViewModel : BaseViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        private long _propertyRoomId;

        public ObservableCollection<PropertyHouseDetails> Rooms { get; }
        public ICommand LoadRoomDetailCommand { get; }

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
        public PropertyHouseRoomDetailViewModel()
        {
            Rooms = new ObservableCollection<PropertyHouseDetails>();
            LoadRoomDetailCommand = new Command(async () => await LoadRoomDetails());
        }

        // Constructor with parameter
        public PropertyHouseRoomDetailViewModel(Services.ServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
        }

        public void SetPropertyRoomId(long propertyRoomId)
        {
            _propertyRoomId = propertyRoomId;
            LoadRoomDetailCommand.Execute(null);
        }

        private async Task LoadRoomDetails()
        {
            IsLoading = true;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousedetaildatabypropertyidandownerid/" + _propertyRoomId, HttpMethod.Get, null);
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
