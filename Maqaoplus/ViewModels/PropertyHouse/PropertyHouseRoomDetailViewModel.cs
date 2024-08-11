﻿using DBL.Entities;
using DBL.Enum;
using DBL.Models;
using Maqaoplus.Views;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouse
{
    public class PropertyHouseRoomDetailViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        private long _propertyRoomId;
        private long _propertyRoomTenantId;
        private string _searchId;
        private Systempropertyhouserooms _houseroomData;
        private SystemStaff _tenantStaffData;
        public event PropertyChangedEventHandler PropertyChanged;


        private decimal _openingMeter;
        private decimal _closingMeter;
        private decimal _movedMeter;
        private decimal _consumedAmount;

        private bool _isStep1Visible;
        private bool _isStep2Visible;
        private bool _isStep3Visible;
        private bool _isStep4Visible;

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
        private bool _isProcessing;
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged();
                ((Command)SearchCommand).ChangeCanExecute();
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
        private bool _isValid;
        public bool IsValid
        {
            get => _isValid;
            private set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged();
                }
            }
        }

        private ObservableCollection<ListModel> _systemkitchentype;
        private ObservableCollection<ListModel> _systempropertyhousesize;

        public ICommand LoadItemsCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand SaveCommand { get; }
        public string SearchId
        {
            get => _searchId;
            set
            {
                _searchId = value;
                OnPropertyChanged();
            }
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
                OnPropertyChanged();
            }
        }
        public void SetPropertyRoomId(long propertyRoomId)
        {
            _propertyRoomId = propertyRoomId;
            LoadItemsCommand.Execute(null);
        }
        public long PropertyRoomTenantId
        {
            get => _propertyRoomTenantId;
            set
            {
                _propertyRoomTenantId = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<ListModel> Systemkitchentype
        {
            get => _systemkitchentype;
            set
            {
                _systemkitchentype = value;
                OnPropertyChanged();
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
                        HouseroomData.Kitchentypeid = 0;
                    }

                    OnPropertyChanged(nameof(SelectedKitchentype));
                    OnPropertyChanged(nameof(HouseroomData.Kitchentypeid));
                }
            }
        }


        public ObservableCollection<ListModel> Systempropertyhousesize
        {
            get => _systempropertyhousesize;
            set
            {
                _systempropertyhousesize = value;
                OnPropertyChanged();
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
                        HouseroomData.Systempropertyhousesizeid = 0;
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
                OnPropertyChanged();
            }
        }

        private string _propertyHouseKitchenTypeError;
        public string PropertyHouseKitchenTypeError
        {
            get => _propertyHouseKitchenTypeError;
            set
            {
                _propertyHouseKitchenTypeError = value;
                OnPropertyChanged();
            }
        }

        private string _propertyHouseSizeError;
        public string PropertyHouseSizeError
        {
            get => _propertyHouseSizeError;
            set
            {
                _propertyHouseSizeError = value;
                OnPropertyChanged();
            }
        }
        public PropertyHouseRoomDetailViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadItemsCommand = new Command(async () => await LoadRoomDetails());
            NextCommand = new Command(NextStep);
            PreviousCommand = new Command(PreviousStep);
            SearchCommand = new Command(async () => await Search());
            SaveCommand = new Command(async () => await SaveRoomDetails());

            // Initialize steps
            _isStep1Visible = true;
            _isStep2Visible = false;
            _isStep3Visible = false;
            _isStep4Visible = false;
            LoadDropdownData();
        }

        private async Task LoadRoomDetails()
        {
            IsLoading = true;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>($"/api/PropertyHouse/Getsystempropertyhouseroomdatabyid/" + _propertyRoomId, HttpMethod.Get, null);

                if (response != null)
                {
                    HouseroomData = JsonConvert.DeserializeObject<Systempropertyhouserooms>(response.Data.ToString());
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
        private async Task LoadDropdownData()
        {
            try
            {
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
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task Search()
        {
            if (IsProcessing || string.IsNullOrWhiteSpace(SearchId))
                return;

            IsLoading = true;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>($"/api/Account/Getsystemstaffdetaildatabyidnumber/" + SearchId, HttpMethod.Get, null);

                if (response != null)
                {
                    TenantStaffData = JsonConvert.DeserializeObject<SystemStaff>(response.Data.ToString());

                    // Navigate to the modal with the customer data
                    var modalPage = new StaffDetailModalPage(
                        TenantStaffData,
                        new Command(OnOkClicked),
                        new Command(OnCancelClicked)
                    );
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
                IsLoading = false;
            }
        }
        private async Task SaveRoomDetails()
        {
            IsLoading = true;
            if (IsProcessing || PropertyRoomTenantId == 0)
                return;

            var aggregatedData = new Systempropertyhouserooms
            {
                Systempropertyhouseroomid = HouseroomData.Systempropertyhouseroomid,
                Systempropertyhouseid = HouseroomData.Systempropertyhouseid,
                Systempropertyhousesizeid = HouseroomData.Systempropertyhousesizeid,
                Systempropertyhousesizename = HouseroomData.Systempropertyhousesizename,
                Isvacant = HouseroomData.Isvacant,
                Isunderrenovation = HouseroomData.Isunderrenovation,
                Isshop = HouseroomData.Isshop,
                Isgroundfloor = HouseroomData.Isgroundfloor,
                Hasbalcony = HouseroomData.Hasbalcony,
                Forcaretaker = HouseroomData.Forcaretaker,
                Kitchentypeid = HouseroomData.Kitchentypeid,
                Systempropertyhousemeterid = HouseroomData.Systempropertyhousemeterid,
                Systempropertyhouseroommeternumber = HouseroomData.Systempropertyhouseroommeternumber,
                Openingmeter = HouseroomData.Openingmeter,
                Movedmeter = MovedMeter,
                Closingmeter = ClosingMeter,
                Consumedamount = ConsumedAmount,
                Tenantid = PropertyRoomTenantId,
                Createdby = App.UserDetails.Usermodel.Userid,
                Datecreated = DateTime.Now,
                Meterhistorydata = HouseroomData.Meterhistorydata
            };

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Registerpropertyhouseroomdata", HttpMethod.Post, JsonConvert.SerializeObject(aggregatedData));

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
                IsLoading = false;
            }
        }
        public decimal OpeningMeter
        {
            get => _openingMeter;
            set
            {
                _openingMeter = value;
                OnPropertyChanged();
                CalculateMeterValues();
            }
        }
        public decimal ClosingMeter
        {
            get => _closingMeter;
            set
            {
                _closingMeter = value;
                OnPropertyChanged();
                if (ClosingMeter > 0)
                {
                    CalculateMeterValues();
                }
            }
        }

        public decimal MovedMeter
        {
            get => _movedMeter;
            set
            {
                _movedMeter = value;
                OnPropertyChanged();
            }
        }

        public decimal ConsumedAmount
        {
            get => _consumedAmount;
            set
            {
                _consumedAmount = value;
                OnPropertyChanged();
            }
        }


        private void CalculateMeterValues()
        {
            if (HouseroomData.Openingmeter >= 0 && ClosingMeter > 0)
            {
                MovedMeter = ClosingMeter - HouseroomData.Openingmeter;
                ConsumedAmount = MovedMeter * HouseroomData.Waterunitprice;
            }
        }
        private void OnOkClicked()
        {
            PropertyRoomTenantId = TenantStaffData.Userid;
            SearchId = string.Empty;
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void OnCancelClicked()
        {
            PropertyRoomTenantId = 0;
            SearchId = string.Empty;
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        public bool IsStep1Visible
        {
            get => _isStep1Visible;
            set
            {
                _isStep1Visible = value;
                OnPropertyChanged();
            }
        }

        public bool IsStep2Visible
        {
            get => _isStep2Visible;
            set
            {
                _isStep2Visible = value;
                OnPropertyChanged();
            }
        }

        public bool IsStep3Visible
        {
            get => _isStep3Visible;
            set
            {
                _isStep3Visible = value;
                OnPropertyChanged();
            }
        }

        public bool IsStep4Visible
        {
            get => _isStep4Visible;
            set
            {
                _isStep4Visible = value;
                OnPropertyChanged();
            }
        }
        private async void NextStep()
        {
            IsLoading = true;

            await Task.Delay(500);
            // Move to the next step
            if (_isStep1Visible)
            {
                if (!ValidateStep1())
                {
                    IsLoading = false;
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
            IsLoading = false;
            OnPropertyChanged(nameof(IsStep1Visible));
            OnPropertyChanged(nameof(IsStep2Visible));
            OnPropertyChanged(nameof(IsStep3Visible));
            OnPropertyChanged(nameof(IsStep4Visible));
        }

        private async void PreviousStep()
        {
            IsLoading = true;

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
            IsLoading = false;

            OnPropertyChanged(nameof(IsStep1Visible));
            OnPropertyChanged(nameof(IsStep2Visible));
            OnPropertyChanged(nameof(IsStep3Visible));
            OnPropertyChanged(nameof(IsStep4Visible));
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
            IsValid = isValid;

            return isValid;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}