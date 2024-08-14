﻿using DBL.Entities;
using DBL.Enum;
using DBL.Models;
using Maqaoplus.Views;
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
        private SystemStaff _tenantStaffData;
        public ICommand LoadRoomsCommand { get; }
        public ICommand ViewRoomDetailsCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand OnCancelClickedCommand { get; }
        public ICommand OnOkClickedCommand { get; }
        public ICommand SearchCommand { get; }

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
            NextCommand = new Command(NextStep);
            PreviousCommand = new Command(PreviousStep);
            SearchCommand = new Command(async () => await Search());
            OnCancelClickedCommand = new Command(OnCancelClicked);
            OnOkClickedCommand = new Command(async () => await SaveHouseRoomDetailsAsync());

            // Initialize steps
            _isStep1Visible = true;
            _isStep2Visible = false;
            _isStep3Visible = false;
            _isStep4Visible = false;
        }
        public bool IsStep1Visible
        {
            get => _isStep1Visible;
            set
            {
                _isStep1Visible = value;
                OnPropertyChanged(nameof(IsStep1Visible));
            }
        }

        public bool IsStep2Visible
        {
            get => _isStep2Visible;
            set
            {
                _isStep2Visible = value;
                OnPropertyChanged(nameof(IsStep2Visible));
            }
        }

        public bool IsStep3Visible
        {
            get => _isStep3Visible;
            set
            {
                _isStep3Visible = value;
                OnPropertyChanged(nameof(IsStep3Visible));
            }
        }

        public bool IsStep4Visible
        {
            get => _isStep4Visible;
            set
            {
                _isStep4Visible = value;
                OnPropertyChanged(nameof(IsStep4Visible));
            }
        }
        private async void NextStep()
        {
            IsProcessing = true;

            await Task.Delay(500);
            // Move to the next step
            if (_isStep1Visible)
            {
                if (!ValidateStep1())
                {
                    IsProcessing = false;
                    return;
                }
                _isStep1Visible = false;
                _isStep2Visible = true;
            }
            else if (_isStep2Visible)
            {
                _isStep2Visible = false;
                _isStep3Visible = true;
            }
            else if (_isStep3Visible)
            {
                _isStep3Visible = false;
                _isStep4Visible = true;
            }
            IsProcessing = false;
            OnPropertyChanged(nameof(IsStep1Visible));
            OnPropertyChanged(nameof(IsStep2Visible));
            OnPropertyChanged(nameof(IsStep3Visible));
            OnPropertyChanged(nameof(IsStep4Visible));
        }

        private async void PreviousStep()
        {
            IsProcessing = true;

            await Task.Delay(500);
            // Move to the previous step
            if (_isStep4Visible)
            {
                _isStep4Visible = false;
                _isStep3Visible = true;
            }
            else if (_isStep3Visible)
            {
                _isStep3Visible = false;
                _isStep2Visible = true;
            }
            else if (_isStep2Visible)
            {
                _isStep2Visible = false;
                _isStep1Visible = true;
            }
            IsProcessing = false;

            OnPropertyChanged(nameof(IsStep1Visible));
            OnPropertyChanged(nameof(IsStep2Visible));
            OnPropertyChanged(nameof(IsStep3Visible));
            OnPropertyChanged(nameof(IsStep4Visible));
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
        public SystemStaff TenantStaffData
        {
            get => _tenantStaffData;
            set
            {
                _tenantStaffData = value;
                OnPropertyChanged(nameof(TenantStaffData));
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
                    IsProcessing = false;
                }
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
        private bool _isStep1Visible;
        private bool _isStep2Visible;
        private bool _isStep3Visible;
        private bool _isStep4Visible;


        public string _searchId;
        public string SearchId
        {
            get => _searchId;
            set
            {
                _searchId = value;
                OnPropertyChanged(nameof(SearchId));
            }
        }

        public long _tenantid;
        public long Tenantid
        {
            get => _tenantid;
            set
            {
                _tenantid = value;
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
        // Error properties
        private string _propertyHouseRoomNumberError;
        public string PropertyHouseRoomNumberError
        {
            get => _propertyHouseRoomNumberError;
            set
            {
                _propertyHouseRoomNumberError = value;
                OnPropertyChanged(nameof(PropertyHouseRoomNumberError));
            }
        }

        private string _propertyHouseKitchenTypeError;
        public string PropertyHouseKitchenTypeError
        {
            get => _propertyHouseKitchenTypeError;
            set
            {
                _propertyHouseKitchenTypeError = value;
                OnPropertyChanged(nameof(PropertyHouseKitchenTypeError));
            }
        }

        private string _propertyHouseSizeError;
        public string PropertyHouseSizeError
        {
            get => _propertyHouseSizeError;
            set
            {
                _propertyHouseSizeError = value;
                OnPropertyChanged(nameof(PropertyHouseSizeError));
            }
        }

        private async Task Search()
        {
            if (IsProcessing || string.IsNullOrWhiteSpace(SearchId))
                return;

            IsProcessing = true;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>($"/api/Account/Getsystemstaffdetaildatabyidnumber/" + SearchId, HttpMethod.Get, null);

                if (response != null)
                {
                    TenantStaffData = JsonConvert.DeserializeObject<SystemStaff>(response.Data.ToString());

                    // Navigate to the modal with the customer data
                    var modalPage = new StaffDetailModalPage(this);
                    await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
                }
                else
                {
                    TenantStaffData = new SystemStaff();
                }
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

        private async Task SaveHouseRoomDetailsAsync()
        {
            IsProcessing = true;
            if (Tenantid == 0)
            {
                IsProcessing = false;
                return;
            }
            if (!ValidateStep1())
            {
                IsProcessing = false;
                return;
            }
            if (HouseroomData == null)
            {
                IsProcessing = false;
                return;
            }

            await Task.Delay(500);
            if (HouseroomData == null)
            {
                IsProcessing = false;
                return;
            }
            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Registerpropertyhouseroomdata", HttpMethod.Post, HouseroomData);

                if (response != null)
                {
                    // Handle response and show success message if needed
                }
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

        private void OnCancelClicked()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private bool ValidateStep1()
        {
            bool isValid = true;

            // Validate Property Name
            if (string.IsNullOrWhiteSpace(HouseroomData?.Systempropertyhousesizename))
            {
                PropertyHouseRoomNumberError = "Property House Number is required.";
                isValid = false;
            }
            else
            {
                PropertyHouseRoomNumberError = null;
            }
            // Validate Property House Water Type
            if (HouseroomData?.Kitchentypeid == 0)
            {
                PropertyHouseKitchenTypeError = "Property House Kitchen Type is required.";
                isValid = false;
            }
            else
            {
                PropertyHouseKitchenTypeError = null;
            }
            // Validate Property House County
            if (HouseroomData?.Systempropertyhousesizeid == 0)
            {
                PropertyHouseSizeError = "Property House Size is required.";
                isValid = false;
            }
            else
            {
                PropertyHouseSizeError = null;
            }
            // Update overall IsValid property
            IsProcessing = isValid;

            return isValid;
        }


    }
}