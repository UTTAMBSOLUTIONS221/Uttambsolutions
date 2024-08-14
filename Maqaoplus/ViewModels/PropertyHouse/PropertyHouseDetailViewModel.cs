using DBL.Entities;
using DBL.Enum;
using DBL.Models;
using Maqaoplus.Views.PropertyHouse.Modal;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouse
{
    public class PropertyHouseDetailViewModel : BaseViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        private long _propertyId;

        public ObservableCollection<PropertyHouseDetails> Rooms { get; }
        private Systempropertyhouserooms _houseroomData;
        public ICommand LoadRoomsCommand { get; }
        public ICommand ViewRoomDetailsCommand { get; }
        public ICommand OnOkClickedCommand { get; }

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
        public PropertyHouseDetailViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Rooms = new ObservableCollection<PropertyHouseDetails>();
            LoadRoomsCommand = new Command(async () => await LoadRooms());
            ViewRoomDetailsCommand = new Command<PropertyHouseDetails>(async (propertyRoom) => await ViewDetails(propertyRoom.Systempropertyhouseroomid));
            OnOkClickedCommand = new Command(async () => await SaveHouseRoomDetailsAsync());

        }

        public void SetPropertyId(long propertyId)
        {
            _propertyId = propertyId;
            LoadRoomsCommand.Execute(null);
        }

        public Systempropertyhouserooms HouseroomData
        {
            get => _houseroomData;
            set
            {
                _houseroomData = value;
                OnPropertyChanged();
            }
        }


        private async Task LoadRooms()
        {
            IsProcessing = true;
            IsDataLoaded = false;

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
                    IsDataLoaded = true; // Data is loaded after processing
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsProcessing = false; // Stop loading indicator
            }
        }

        private async Task ViewDetails(long propertyRoomId)
        {
            IsProcessing = true;
            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>($"/api/PropertyHouse/Getsystempropertyhouseroomdatabyid/" + propertyRoomId, HttpMethod.Get, null);

                if (response != null && response.Data != null)
                {
                    HouseroomData = JsonConvert.DeserializeObject<Systempropertyhouserooms>(response.Data.ToString());
                    var kitchentypeResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.Systemkitchentype, HttpMethod.Get);
                    var sizeResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.Systempropertyhousesizes, HttpMethod.Get);

                    if (kitchentypeResponse != null)
                    {
                        Systemkitchentype = new ObservableCollection<ListModel>(kitchentypeResponse);
                    }

                    if (sizeResponse != null)
                    {
                        Systempropertyhousesize = new ObservableCollection<ListModel>(sizeResponse);
                    }

                    var modalPage = new HousesRoomDetailModalPage(this);
                    await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
                }

                //var encodedPropertyRoomId = Uri.EscapeDataString(propertyRoomId.ToString());
                //await Shell.Current.GoToAsync($"PropertyHousesRoomDetailPage?PropertyRoomId={encodedPropertyRoomId}");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Navigation Error", ex.Message, "OK");
            }
            finally
            {
                IsProcessing = false; // Stop loading indicator
            }
        }

        //maibupation of the House Room Details
        public long _systempropertyhouseroomid;
        public long Systempropertyhouseroomid
        {
            get => _systempropertyhouseroomid;
            set
            {
                _systempropertyhouseroomid = value;
                OnPropertyChanged();
                ((Command)OnOkClickedCommand).ChangeCanExecute();
            }
        }

        public long _propertyRoomTenantId;
        public long PropertyRoomTenantId
        {
            get => _propertyRoomTenantId;
            set
            {
                _propertyRoomTenantId = value;
                OnPropertyChanged();
                ((Command)OnOkClickedCommand).ChangeCanExecute();
            }
        }

        public string _systempropertyhousesizename;
        public string Systempropertyhousesizename
        {
            get => _systempropertyhousesizename;
            set
            {
                _systempropertyhousesizename = value;
                OnPropertyChanged();
                ((Command)OnOkClickedCommand).ChangeCanExecute();
            }
        }
        private ObservableCollection<ListModel> _systemkitchentype;
        public ObservableCollection<ListModel> Systemkitchentype
        {
            get => _systemkitchentype;
            set
            {
                _systemkitchentype = value;
                OnPropertyChanged(nameof(Systemkitchentype));
            }
        }
        private ListModel _selectedKitchentype;
        public ListModel SelectedKitchentype
        {
            get => _selectedKitchentype;
            set
            {
                _selectedKitchentype = value;

                // Ensure SystempropertyData is not null
                if (HouseroomData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int kitchentypeid))
                    {
                        HouseroomData.Kitchentypeid = kitchentypeid;
                    }
                    else
                    {
                        HouseroomData.Kitchentypeid = HouseroomData.Kitchentypeid;
                    }

                    OnPropertyChanged(nameof(SelectedKitchentype));
                    OnPropertyChanged(nameof(HouseroomData.Kitchentypeid));
                }
            }
        }

        private ObservableCollection<ListModel> _systempropertyhousesize;
        public ObservableCollection<ListModel> Systempropertyhousesize
        {
            get => _systempropertyhousesize;
            set
            {
                _systempropertyhousesize = value;
                OnPropertyChanged(nameof(Systempropertyhousesize));
            }
        }
        private ListModel _selectedPropertyhousesize;
        public ListModel SelectedPropertyhousesize
        {
            get => _selectedPropertyhousesize;
            set
            {
                _selectedPropertyhousesize = value;

                // Ensure SystempropertyData is not null
                if (HouseroomData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int systempropertyhousesizeid))
                    {
                        HouseroomData.Systempropertyhousesizeid = systempropertyhousesizeid;
                    }
                    else
                    {
                        HouseroomData.Systempropertyhousesizeid = HouseroomData.Systempropertyhousesizeid;
                    }

                    OnPropertyChanged(nameof(SelectedPropertyhousesize));
                    OnPropertyChanged(nameof(HouseroomData.Systempropertyhousesizeid));
                }
            }
        }







        private async Task SaveHouseRoomDetailsAsync()
        {
            var data = new Systempropertyhouserooms()
            {

            };
        }

    }
}