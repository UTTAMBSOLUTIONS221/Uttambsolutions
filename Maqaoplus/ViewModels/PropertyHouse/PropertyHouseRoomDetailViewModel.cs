using DBL.Entities;
using DBL.Enum;
using DBL.Models;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouse
{
    public class PropertyHouseRoomDetailViewModel : BaseViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        private long _propertyRoomId;
        private Systempropertyhouserooms _houseroomData;
        private bool _isLoading;
        private ObservableCollection<ListModel> _systemkitchentype;
        private ListModel _selectedKitchentype;
        private ObservableCollection<ListModel> _systempropertyhousesize;
        private ListModel _selectedSize;
        private string _systempropertyhousesizename;
        private bool _continueWithoutTenant;
        private bool _forcaretaker;
        private bool _isshop;
        private bool _isgroundfloor;
        private bool _hasbalcony;
        private bool _isunderrenovation;
        private ObservableCollection<Systempropertyhouseroommeterhistory> _meterReadings;
        private string _searchId;
        private string _searchResults;

        private string _openingMeter;
        private string _closingMeter;
        private string _movedMeter;
        private string _consumedAmount;
        private bool _isOpeningMeterReadOnly;

        private bool _isStep1Visible;
        private bool _isStep2Visible;
        private bool _isStep3Visible;
        private bool _isStep4Visible;

        public ICommand LoadRoomDetailCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand CloseCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }

        public PropertyHouseRoomDetailViewModel()
        {
            LoadRoomDetailCommand = new Command(async () => await LoadRoomDetails());
            SaveCommand = new Command(async () => await SaveRoomDetails());
            SearchCommand = new Command(async () => await Search());
            NextCommand = new Command(NextStep);
            PreviousCommand = new Command(PreviousStep);

            // Initialize steps
            _isStep1Visible = true;
            _isStep2Visible = false;
            _isStep3Visible = false;
            _isStep4Visible = false;
        }

        public PropertyHouseRoomDetailViewModel(Services.ServiceProvider serviceProvider) : this()
        {
            _serviceProvider = serviceProvider;
        }

        public void SetPropertyRoomId(long propertyRoomId)
        {
            _propertyRoomId = propertyRoomId;
            LoadRoomDetailCommand.Execute(null);
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

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
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

        public ListModel SelectedKitchentype
        {
            get => _selectedKitchentype;
            set
            {
                _selectedKitchentype = value;
                OnPropertyChanged();
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

        public ListModel SelectedSize
        {
            get => _selectedSize;
            set
            {
                _selectedSize = value;
                OnPropertyChanged();
            }
        }

        public string Systempropertyhousesizename
        {
            get => _systempropertyhousesizename;
            set
            {
                _systempropertyhousesizename = value;
                OnPropertyChanged();
            }
        }



        public bool Forcaretaker
        {
            get => _forcaretaker;
            set
            {
                _forcaretaker = value;
                OnPropertyChanged();
            }
        }

        public bool Isshop
        {
            get => _isshop;
            set
            {
                _isshop = value;
                OnPropertyChanged();
            }
        }

        public bool Isgroundfloor
        {
            get => _isgroundfloor;
            set
            {
                _isgroundfloor = value;
                OnPropertyChanged();
            }
        }

        public bool Hasbalcony
        {
            get => _hasbalcony;
            set
            {
                _hasbalcony = value;
                OnPropertyChanged();
            }
        }

        public bool Isunderrenovation
        {
            get => _isunderrenovation;
            set
            {
                _isunderrenovation = value;
                OnPropertyChanged();
            }
        }
        public string OpeningMeter
        {
            get => _openingMeter;
            set
            {
                if (_openingMeter != value)
                {
                    _openingMeter = value;
                    OnPropertyChanged();
                    UpdateReadOnlyStatus();
                    CalculateMovedMeter();
                    CalculateConsumedAmount();
                }
            }
        }

        public string ClosingMeter
        {
            get => _closingMeter;
            set
            {
                if (_closingMeter != value)
                {
                    _closingMeter = value;
                    OnPropertyChanged();
                    CalculateMovedMeter();
                    CalculateConsumedAmount();
                }
            }
        }

        public string MovedMeter
        {
            get => _movedMeter;
            set
            {
                if (_movedMeter != value)
                {
                    _movedMeter = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ConsumedAmount
        {
            get => _consumedAmount;
            set
            {
                if (_consumedAmount != value)
                {
                    _consumedAmount = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsOpeningMeterReadOnly
        {
            get => _isOpeningMeterReadOnly;
            set
            {
                if (_isOpeningMeterReadOnly != value)
                {
                    _isOpeningMeterReadOnly = value;
                    OnPropertyChanged();
                }
            }
        }

        private void UpdateReadOnlyStatus()
        {
            IsOpeningMeterReadOnly = OpeningMeter != "0";
        }

        private void CalculateMovedMeter()
        {
            if (decimal.TryParse(ClosingMeter, out var closing) &&
                decimal.TryParse(OpeningMeter, out var opening))
            {
                MovedMeter = (closing - opening).ToString();
            }
        }

        private void CalculateConsumedAmount()
        {
            if (decimal.TryParse(MovedMeter, out var moved) &&
                decimal.TryParse(UnitPrice, out var unitPrice))
            {
                ConsumedAmount = (moved * unitPrice).ToString();
            }
        }

        public string UnitPrice { get; set; } = "1"; // Example value, replace with actual

        public ObservableCollection<Systempropertyhouseroommeterhistory> MeterReadings
        {
            get => _meterReadings;
            set
            {
                _meterReadings = value;
                OnPropertyChanged();
            }
        }

        public string SearchId
        {
            get => _searchId;
            set
            {
                _searchId = value;
                OnPropertyChanged();
            }
        }

        public string SearchResults
        {
            get => _searchResults;
            set
            {
                _searchResults = value;
                OnPropertyChanged();
            }
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

        private async Task LoadRoomDetails()
        {
            IsLoading = true;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>($"/api/PropertyHouse/Getsystempropertyhouseroomdatabyid/" + _propertyRoomId, HttpMethod.Get, null);

                if (response != null)
                {
                    HouseroomData = JsonConvert.DeserializeObject<Systempropertyhouserooms>(response.Data.ToString());
                    await LoadDropdownData();
                    // Also ensure meter readings are loaded if applicable
                    await LoadMeterReadings();
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

        private async Task LoadMeterReadings()
        {
            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>($"/api/PropertyHouse/GetMeterReadingsByRoomId/" + _propertyRoomId, HttpMethod.Get, null);

                if (response != null)
                {
                    MeterReadings = JsonConvert.DeserializeObject<ObservableCollection<Systempropertyhouseroommeterhistory>>(response.Data.ToString());
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task SaveRoomDetails()
        {
            IsLoading = true;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/SaveRoomDetails", HttpMethod.Post, JsonConvert.SerializeObject(HouseroomData));

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

        private async Task Search()
        {
            // Implement search logic
        }


        private void NextStep()
        {
            // Move to the next step
            if (_isStep1Visible)
            {
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
            OnPropertyChanged(nameof(IsStep1Visible));
            OnPropertyChanged(nameof(IsStep2Visible));
            OnPropertyChanged(nameof(IsStep3Visible));
            OnPropertyChanged(nameof(IsStep4Visible));
        }

        private void PreviousStep()
        {
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
            OnPropertyChanged(nameof(IsStep1Visible));
            OnPropertyChanged(nameof(IsStep2Visible));
            OnPropertyChanged(nameof(IsStep3Visible));
            OnPropertyChanged(nameof(IsStep4Visible));
        }
    }
}
