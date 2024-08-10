using DBL.Entities;
using DBL.Enum;
using DBL.Models;
using Maqaoplus.Views;
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
        private SystemStaff _tenantStaffData;
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
        private string _searchId;
        private bool _isProcessing;
        private string _searchResults;

        private string _openingMeter;
        private string _closingMeter;
        private string _movedMeter;
        private string _consumedAmount;
        private bool _isOpeningMeterReadOnly;

        private bool _isStep1Visible;
        private bool _isStep2Visible;
        private bool _isStep3Visible;

        private bool _isDisposed;


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

        public SystemStaff TenantStaffData
        {
            get => _tenantStaffData;
            set
            {
                _tenantStaffData = value;
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
        public void Dispose()
        {
            _isDisposed = true;
        }
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
        public string UnitPrice { get; set; } = "1"; // Example value, replace with actual

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

        private async Task LoadRoomDetails()
        {
            if (_isDisposed)
                return;

            IsLoading = true;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>($"/api/PropertyHouse/Getsystempropertyhouseroomdatabyid/" + _propertyRoomId, HttpMethod.Get, null);

                if (response != null)
                {
                    HouseroomData = JsonConvert.DeserializeObject<Systempropertyhouserooms>(response.Data.ToString());
                    await LoadDropdownData();
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
            if (_isDisposed)
                return;

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
        private async Task SaveRoomDetails()
        {
            if (_isDisposed)
                return;
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
                    SearchResults = "No results found.";
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

        private void OnOkClicked()
        {
            // Update the form entry with the tenant ID or other relevant data
            // Example: PropertyIdEntry.Text = TenantStaffData.Id;
            // Close the modal
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void OnCancelClicked()
        {
            // Do nothing or handle cancel logic
            Application.Current.MainPage.Navigation.PopModalAsync();
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
            OnPropertyChanged(nameof(IsStep1Visible));
            OnPropertyChanged(nameof(IsStep2Visible));
            OnPropertyChanged(nameof(IsStep3Visible));
        }

        private void PreviousStep()
        {
            // Move to the previous step
            if (_isStep3Visible)
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
        }
    }
}