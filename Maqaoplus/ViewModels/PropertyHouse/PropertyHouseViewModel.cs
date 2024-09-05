using DBL.Entities;
using DBL.Enum;
using DBL.Models;
using Maqaoplus.Views;
using Maqaoplus.Views.PropertyHouse;
using Maqaoplus.Views.PropertyHouse.Modal;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouse
{
    public class PropertyHouseViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public event PropertyChangedEventHandler PropertyChanged;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        public string AgreementText { get; set; }
        public ObservableCollection<Systemproperty> Items { get; }
        public ObservableCollection<PropertyHouseDetails> Rooms { get; }
        public ObservableCollection<SystemStaff> PropertyHouseCareTakerItems { get; }
        public ObservableCollection<PropertyHouseDetails> VacantItems { get; }
        private Systemproperty _systempropertyData;
        private Systempropertyhouserooms _houseroomData;
        private OwnerTenantAgreementDetailData _ownerTenantAgreementDetailData;
        private SystemPropertyHouseImage _systemPropertyHouseImageData;
        private Systemtenantdetails _tenantStaffData;
        private SystemStaff _systemstaffdata;
        private Systemtenantdetails _newTenantStaffData;
        private Systempropertyhouseroomfixtures _systempropertyhouseroomfixturesData;

        private decimal _openingMeter;
        private decimal _closingMeter;
        private decimal _movedMeter;
        private decimal _consumedAmount;

        private bool _isStep1HouseVisible;
        private bool _isStep2HouseVisible;
        private bool _isStep3HouseVisible;
        private bool _isStep4HouseVisible;
        private bool _isStep5HouseVisible;

        private bool _isStep1HouseRoomVisible;
        private bool _isStep2HouseRoomVisible;
        private bool _isStep3HouseRoomVisible;
        private bool _isStep4HouseRoomVisible;
        private string _step2HouseRoomLabel;
        private string _step3HouseRoomLabel;

        public ICommand AddPropertyHouseCommand { get; }
        public ICommand AddAgentPropertyHouseCommand { get; }
        public ICommand AddPropertyHouseCareTakerCommand { get; }
        public ICommand LoadItemsCommand { get; }

        public ICommand LoadPropertyHouseCaretakerItemsCommand { get; }
        public ICommand LoadVacantPropertyHousesCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand LoadMoreCommand { get; }
        public ICommand LoadAgentItemsCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand ViewPropertyHouseImageCommand { get; }
        public ICommand HouseNextCommand { get; }
        public ICommand HousePreviousCommand { get; }
        public ICommand HouseRoomNextCommand { get; }
        public ICommand HouseRoomPreviousCommand { get; }
        public ICommand OnCancelClickedCommand { get; }
        public ICommand SavePropertyHouseCommand { get; }
        public ICommand OnCancelCareTakerButtonClickedCommand { get; }
        public ICommand OnOkCareTakerButtonClickedCommand { get; }
        public ICommand SaveAgentPropertyHouseCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand SearchStaffsCommand { get; }
        public ICommand SavePropertyHouseCareTakerCommand { get; }

        public ICommand OnHouseRoomCancelClickedCommand { get; }
        public ICommand OnHouseRoomOkClickedCommand { get; }


        public ICommand ViewRoomDetailsCommand { get; }
        public ICommand ViewPropertyRoomCheckListCommand { get; }
        public ICommand ViewPropertyRoomImageCommand { get; }
        public ICommand SavePropertyHouseRoomFixtureCommand { get; }


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
        private int _pageNumber = 1;
        private bool _isLoadingMore;

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

        private ObservableCollection<ListModel> _systemcounty;
        private ObservableCollection<ListModel> _systemsubcounty;
        private ObservableCollection<ListModel> _systemsubcountyward;
        private ObservableCollection<ListModel> _systemhouseentrystatus;
        private ObservableCollection<ListModel> _systemhousewatertype;
        private ObservableCollection<ListModel> _systemhouserentdueday;
        private ObservableCollection<ListModel> _systemhouserentdepositmonths;
        private ObservableCollection<ListModel> _systemhouserentdepositreturndays;
        private ObservableCollection<ListModel> _systemhousevacantnoticeperiod;
        private ObservableCollection<ListModel> _systemhouserentingterms;
        private ObservableCollection<ListModel> _systemownerhouse;

        public Systemproperty SystempropertyData
        {
            get => _systempropertyData;
            set
            {
                if (_systempropertyData != value)
                {
                    if (_systempropertyData != null)
                    {
                        // Unsubscribe from previous instance's property change events
                        _systempropertyData.PropertyChanged -= OnSystempropertyDataChangedAsync;
                    }

                    _systempropertyData = value;

                    if (_systempropertyData != null)
                    {
                        // Subscribe to new instance's property change events
                        _systempropertyData.PropertyChanged += OnSystempropertyDataChangedAsync;
                    }

                    OnPropertyChanged(nameof(SystempropertyData));
                    OnPropertyChanged(nameof(IsPetsAllowedVisible));
                }
            }
        }
        public bool IsPetsAllowedVisible => SystempropertyData?.Allowpets ?? false;
        private void OnSystempropertyDataChangedAsync(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Systemproperty.Allowpets))
            {
                OnPropertyChanged(nameof(IsPetsAllowedVisible));
            }
            if (e.PropertyName == nameof(Systemproperty.Rentingterms))
            {
                OnPropertyChanged(nameof(IsRentingTermsVisible));
            }

            // Add checks for other properties if needed
        }

        public bool IsRentingTermsVisible
        {
            get
            {
                return SystempropertyData != null && SelectedHouserentingterms?.Value != null && IsFixedTerm(SelectedHouserentingterms.Value);
            }
        }

        private bool IsFixedTerm(string rentingTerms)
        {
            // Define your logic for determining if the term is fixed
            // For example, checking if the rentingTerms contains the word "fixed"
            return !string.IsNullOrEmpty(rentingTerms) && rentingTerms.Contains("Fixedterm", StringComparison.OrdinalIgnoreCase);
        }

        public OwnerTenantAgreementDetailData OwnerTenantAgreementDetailData
        {
            get => _ownerTenantAgreementDetailData;
            set
            {
                _ownerTenantAgreementDetailData = value;
                OnPropertyChanged(nameof(OwnerTenantAgreementDetailData));
                OnPropertyChanged(nameof(IsSignatureDrawingVisible));
                OnPropertyChanged(nameof(IsSignatureImageVisible));
                OnPropertyChanged(nameof(IsSignatureAvailable));
            }
        }

        public bool IsSignatureDrawingVisible => string.IsNullOrEmpty(OwnerTenantAgreementDetailData?.OwnerSignatureimageurl);
        public bool IsSignatureImageVisible => !string.IsNullOrEmpty(OwnerTenantAgreementDetailData?.OwnerSignatureimageurl);
        public bool IsSignatureAvailable => !string.IsNullOrEmpty(OwnerTenantAgreementDetailData?.OwnerSignatureimageurl);
        public Systemtenantdetails TenantStaffData
        {
            get => _tenantStaffData;
            set
            {
                _tenantStaffData = value;
                OnPropertyChanged();
            }
        }

        public SystemStaff Systemstaffdata
        {
            get => _systemstaffdata;
            set
            {
                _systemstaffdata = value;
                OnPropertyChanged();
            }
        }
        public SystemPropertyHouseImage SystemPropertyHouseImageData
        {
            get => _systemPropertyHouseImageData;
            set
            {
                _systemPropertyHouseImageData = value;
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
        public Systemtenantdetails NewTenantStaffData
        {
            get => _newTenantStaffData;
            set
            {
                _newTenantStaffData = value;
                OnPropertyChanged();
            }
        }
        public Systempropertyhouseroomfixtures SystempropertyhouseroomfixturesData
        {
            get => _systempropertyhouseroomfixturesData;
            set
            {
                _systempropertyhouseroomfixturesData = value;
                OnPropertyChanged();
            }
        }

        public string _searchId;
        public string SearchId
        {
            get => _searchId;
            set
            {
                _searchId = value;
                OnPropertyChanged();
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
            }
        }

        private ObservableCollection<ListModel> _systemkitchentype;
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
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ListModel> _systempropertyhousesize;
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
                if (_selectedPropertyhousesize != value)
                {
                    _selectedPropertyhousesize = value;
                    OnPropertyChanged();
                }
            }
        }


        // Error properties
        private string _propertyHouseNameError;
        public string PropertyHouseNameError
        {
            get => _propertyHouseNameError;
            set
            {
                _propertyHouseNameError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHouseMonthlycollectionError;
        public string PropertyHouseMonthlycollectionError
        {
            get => _propertyHouseMonthlycollectionError;
            set
            {
                _propertyHouseMonthlycollectionError = value;
                OnPropertyChanged();
            }
        }

        private string _streetOrLandmarkError;
        public string StreetOrLandmarkError
        {
            get => _streetOrLandmarkError;
            set
            {
                _streetOrLandmarkError = value;
                OnPropertyChanged();
            }
        }

        private string _contactDetailsError;
        public string ContactDetailsError
        {
            get => _contactDetailsError;
            set
            {
                _contactDetailsError = value;
                OnPropertyChanged();
            }
        }

        private string _propertyHouseStatusError;
        public string PropertyHouseStatusError
        {
            get => _propertyHouseStatusError;
            set
            {
                _propertyHouseStatusError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHouseWaterTypeError;
        public string PropertyHouseWaterTypeError
        {
            get => _propertyHouseWaterTypeError;
            set
            {
                _propertyHouseWaterTypeError = value;
                OnPropertyChanged();
            }
        }


        private string _propertyHouseCountyError;
        public string PropertyHouseCountyError
        {
            get => _propertyHouseCountyError;
            set
            {
                _propertyHouseCountyError = value;
                OnPropertyChanged();
            }
        }

        private string _propertyHouseSubcountyError;
        public string PropertyHouseSubcountyError
        {
            get => _propertyHouseSubcountyError;
            set
            {
                _propertyHouseSubcountyError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHouseSubcountyWardError;
        public string PropertyHouseSubcountyWardError
        {
            get => _propertyHouseSubcountyWardError;
            set
            {
                _propertyHouseSubcountyWardError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHouseRentDueDayError;
        public string PropertyHouseRentDueDayError
        {
            get => _propertyHouseRentDueDayError;
            set
            {
                _propertyHouseRentDueDayError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHouseRentDepositMonthsError;
        public string PropertyHouseRentDepositMonthsError
        {
            get => _propertyHouseRentDepositMonthsError;
            set
            {
                _propertyHouseRentDepositMonthsError = value;
                OnPropertyChanged();
            }
        }

        private string _propertyHouseRentVacationPeriodMonthsError;
        public string PropertyHouseRentVacationPeriodMonthsError
        {
            get => _propertyHouseRentVacationPeriodMonthsError;
            set
            {
                _propertyHouseRentVacationPeriodMonthsError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHouseRentDepositReturnDaysError;
        public string PropertyHouseRentDepositReturnDaysError
        {
            get => _propertyHouseRentDepositReturnDaysError;
            set
            {
                _propertyHouseRentDepositReturnDaysError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHouseRentingTermsError;
        public string PropertyHouseRentingTermsError
        {
            get => _propertyHouseRentingTermsError;
            set
            {
                _propertyHouseRentingTermsError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHouseRentingTermsEnddateError;
        public string PropertyHouseRentingTermsEnddateError
        {
            get => _propertyHouseRentingTermsEnddateError;
            set
            {
                _propertyHouseRentingTermsEnddateError = value;
                OnPropertyChanged();
            }
        }

        private string _propertyHouseNumberofpetsError;
        public string PropertyHouseNumberofpetsError
        {
            get => _propertyHouseNumberofpetsError;
            set
            {
                _propertyHouseNumberofpetsError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHousePetdepositError;
        public string PropertyHousePetdepositError
        {
            get => _propertyHousePetdepositError;
            set
            {
                _propertyHousePetdepositError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHousePetparticularsError;
        public string PropertyHousePetparticularsError
        {
            get => _propertyHousePetparticularsError;
            set
            {
                _propertyHousePetparticularsError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyOwnerHouseError;
        public string PropertyOwnerHouseError
        {
            get => _propertyOwnerHouseError;
            set
            {
                _propertyOwnerHouseError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHouseCareTakerError;
        public string PropertyHouseCareTakerError
        {
            get => _propertyHouseCareTakerError;
            set
            {
                _propertyHouseCareTakerError = value;
                OnPropertyChanged();
            }
        }

        private string _propertyHouseRoomSizeError;
        public string PropertyHouseRoomSizeError
        {
            get => _propertyHouseRoomSizeError;
            set
            {
                _propertyHouseRoomSizeError = value;
                OnPropertyChanged();
            }
        }

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

        private string _propertyHouseRoomRentError;
        public string PropertyHouseRoomRentError
        {
            get => _propertyHouseRoomRentError;
            set
            {
                _propertyHouseRoomRentError = value;
                OnPropertyChanged();
            }
        }
        private string _propertyHouseRoomoccupantError;
        public string PropertyHouseRoomoccupantError
        {
            get => _propertyHouseRoomoccupantError;
            set
            {
                _propertyHouseRoomoccupantError = value;
                OnPropertyChanged();
            }
        }

        private string _propertyHouseRoomoccupantdetailError;
        public string PropertyHouseRoomoccupantdetailError
        {
            get => _propertyHouseRoomoccupantdetailError;
            set
            {
                _propertyHouseRoomoccupantdetailError = value;
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

        private string _propertyHouseRoomClosingMeterError;
        public string PropertyHouseRoomClosingMeterError
        {
            get => _propertyHouseRoomClosingMeterError;
            set
            {
                _propertyHouseRoomClosingMeterError = value;
                OnPropertyChanged();
            }
        }

        private string _propertyHouseRoomTenantidError;
        public string PropertyHouseRoomTenantidError
        {
            get => _propertyHouseRoomTenantidError;
            set
            {
                _propertyHouseRoomTenantidError = value;
                OnPropertyChanged();
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


        private string _careTakerFullname;
        public string CareTakerFullname
        {
            get => _careTakerFullname;
            set
            {
                _careTakerFullname = value;
                OnPropertyChanged();
            }
        }
        public ObservableCollection<Systempropertyhousesize> PropertyHouseSizes { get; set; } = new ObservableCollection<Systempropertyhousesize>();
        public ObservableCollection<Systempropertyhousedepositfees> PropertyHouseDepositFees { get; set; } = new ObservableCollection<Systempropertyhousedepositfees>();
        public ObservableCollection<Systempropertyhousebenefits> PropertyHouseBenefits { get; set; } = new ObservableCollection<Systempropertyhousebenefits>();

        // Parameterless constructor for XAML support
        public PropertyHouseViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadDropdownData();
            Items = new ObservableCollection<Systemproperty>();
            Rooms = new ObservableCollection<PropertyHouseDetails>();
            PropertyHouseCareTakerItems = new ObservableCollection<SystemStaff>();
            VacantItems = new ObservableCollection<PropertyHouseDetails>();
            AddPropertyHouseCommand = new Command<Systemproperty>(async (property) => { var propertyId = property?.Propertyhouseid ?? 0; await AddPropertyHouseAsync(propertyId); });
            AddAgentPropertyHouseCommand = new Command<Systemproperty>(async (property) => { var propertyId = property?.Propertyhouseid ?? 0; await AddAgentPropertyHouseAsync(propertyId); });
            AddPropertyHouseCareTakerCommand = new Command<SystemStaff>(async (param) => { var caretakerhouseid = param?.Caretakerhouseid ?? 0; await AddPropertyHouseCareTakerAsync(caretakerhouseid); });
            LoadItemsCommand = new Command(async () => await LoadItems());
            LoadPropertyHouseCaretakerItemsCommand = new Command(async () => await LoadPropertyHouseCaretakerItems());
            LoadVacantPropertyHousesCommand = new Command(async () => await LoadVacantPropertyHouses());
            RefreshCommand = new Command(async () => await RefreshItemsAsync());
            LoadMoreCommand = new Command(async () => await LoadMoreItemsAsync());
            LoadAgentItemsCommand = new Command(async () => await LoadAgentItems());
            ViewDetailsCommand = new Command<Systemproperty>(async (property) => await ViewHouseDetails(property.Propertyhouseid));
            ViewPropertyHouseImageCommand = new Command<Systemproperty>(async (property) => await ViewPropertyHouseImagesDetails(property.Propertyhouseid));
            HouseNextCommand = new Command(HouseNextStep);
            HousePreviousCommand = new Command(HousePreviousStep);
            HouseRoomNextCommand = new Command(HouseRoomNextStep);
            HouseRoomPreviousCommand = new Command(HouseRoomPreviousStep);
            OnCancelClickedCommand = new Command(OnCancelClicked);
            SavePropertyHouseCommand = new Command(async () => await SavePropertyHouseAsync());
            OnCancelCareTakerButtonClickedCommand = new Command(OnCancelCareTakerButtonClicked);
            OnOkCareTakerButtonClickedCommand = new Command(async () => await OnOkCareTakerButtonClicked());
            SearchCommand = new Command(async () => await Search());
            SearchStaffsCommand = new Command(async () => await SearchStaff());
            SaveAgentPropertyHouseCommand = new Command(async () => await SaveAgentPropertyHouseAsync());
            SavePropertyHouseCareTakerCommand = new Command(async () => await SavePropertyHouseCareTakerAsync());

            OnHouseRoomCancelClickedCommand = new Command(OnHouseRoomCancelClicked);
            OnHouseRoomOkClickedCommand = new Command(OnHouseRoomOkClicked);


            ViewRoomDetailsCommand = new Command<PropertyHouseDetails>(async (propertyRoom) => await ViewRoomDetails(propertyRoom.Systempropertyhouseroomid));
            ViewPropertyRoomCheckListCommand = new Command<PropertyHouseDetails>(async (propertyRoom) => await ViewPropertyRoomCheckListDetailAsync(propertyRoom.Systempropertyhouseroomid));
            ViewPropertyRoomImageCommand = new Command<PropertyHouseDetails>(async (propertyRoom) => await ViewPropertyRoomImagesDetails(propertyRoom.Systempropertyhouseroomid));

            // Initialize steps
            _isStep1HouseVisible = true;
            _isStep2HouseVisible = false;
            _isStep3HouseVisible = false;
            _isStep4HouseVisible = false;
            _isStep5HouseVisible = false;


            _isStep1HouseRoomVisible = true;
            _isStep2HouseRoomVisible = false;
            _isStep3HouseRoomVisible = false;
            _isStep4HouseRoomVisible = false;
        }

        private async Task RefreshItemsAsync()
        {
            IsProcessing = true;
            _pageNumber = 1;
            VacantItems.Clear();

            // Load the first page of items
            await LoadVacantPropertyHouses();

            IsProcessing = false;
        }

        private async Task LoadMoreItemsAsync()
        {
            if (_isLoadingMore) return;

            _isLoadingMore = true;

            // Load the next page of items
            _pageNumber++;
            await LoadVacantPropertyHouses();

            _isLoadingMore = false;
        }
        public ObservableCollection<ListModel> Systemcounty
        {
            get => _systemcounty;
            set
            {
                _systemcounty = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedCounty;
        public ListModel SelectedCounty
        {
            get => _selectedCounty;
            set
            {
                _selectedCounty = value;
                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int countyId))
                    {
                        SystempropertyData.Countyid = countyId;
                    }
                    else
                    {
                        SystempropertyData.Countyid = 0;
                    }

                    OnPropertyChanged(nameof(SelectedCounty));
                    OnPropertyChanged(nameof(SystempropertyData.Countyid));
                    LoadSubcountyDataCountyCode();
                }
            }
        }
        public ObservableCollection<ListModel> Systemsubcounty
        {
            get => _systemsubcounty;
            set
            {
                _systemsubcounty = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedSubcounty;
        public ListModel SelectedSubcounty
        {
            get => _selectedSubcounty;
            set
            {
                _selectedSubcounty = value;
                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int subCountyId))
                    {
                        SystempropertyData.Subcountyid = subCountyId;
                    }
                    else
                    {
                        SystempropertyData.Countyid = 0;
                    }

                    OnPropertyChanged(nameof(SelectedSubcounty));
                    OnPropertyChanged(nameof(SystempropertyData.Subcountyid));
                    LoadSubcountyWardDataCountyCode();
                }
            }
        }

        public ObservableCollection<ListModel> Systemsubcountyward
        {
            get => _systemsubcountyward;
            set
            {
                _systemsubcountyward = value;
                OnPropertyChanged();
            }
        }

        private ListModel _selectedSubcountyward;
        public ListModel SelectedSubcountyward
        {
            get => _selectedSubcountyward;
            set
            {
                _selectedSubcountyward = value;
                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int subcountywardid))
                    {
                        SystempropertyData.Subcountywardid = subcountywardid;
                    }
                    else
                    {
                        SystempropertyData.Subcountywardid = 0;
                    }

                    OnPropertyChanged(nameof(SelectedSubcountyward));
                    OnPropertyChanged(nameof(SystempropertyData.Subcountywardid));
                }
            }
        }

        public ObservableCollection<ListModel> Systemhouseentrystatus
        {
            get => _systemhouseentrystatus;
            set
            {
                _systemhouseentrystatus = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedHouseentrystatus;
        public ListModel SelectedHouseentrystatus
        {
            get => _selectedHouseentrystatus;
            set
            {
                _selectedHouseentrystatus = value;
                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int propertyhousestatus))
                    {
                        SystempropertyData.Propertyhousestatus = propertyhousestatus;
                    }
                    else
                    {
                        SystempropertyData.Propertyhousestatus = 0;
                    }

                    OnPropertyChanged(nameof(SelectedHouseentrystatus));
                    OnPropertyChanged(nameof(SystempropertyData.Propertyhousestatus));
                }
            }
        }
        public ObservableCollection<ListModel> Systemhousewatertype
        {
            get => _systemhousewatertype;
            set
            {
                _systemhousewatertype = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedHousewatertype;
        public ListModel SelectedHousewatertype
        {
            get => _selectedHousewatertype;
            set
            {
                _selectedHousewatertype = value;
                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int watertypeid))
                    {
                        SystempropertyData.Watertypeid = watertypeid;
                    }
                    else
                    {
                        SystempropertyData.Watertypeid = 0;
                    }

                    OnPropertyChanged(nameof(SelectedHousewatertype));
                    OnPropertyChanged(nameof(SystempropertyData.Watertypeid));
                }
            }
        }
        public ObservableCollection<ListModel> Systemhouserentdueday
        {
            get => _systemhouserentdueday;
            set
            {
                _systemhouserentdueday = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedHouserentdueday;
        public ListModel SelectedHouserentdueday
        {
            get => _selectedHouserentdueday;
            set
            {
                _selectedHouserentdueday = value;
                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int rentdueday))
                    {
                        SystempropertyData.Rentdueday = rentdueday;
                    }
                    else
                    {
                        SystempropertyData.Rentdueday = 0;
                    }

                    OnPropertyChanged(nameof(SelectedHouserentdueday));
                    OnPropertyChanged(nameof(SystempropertyData.Rentdueday));
                }
            }
        }
        public ObservableCollection<ListModel> Systemhousedepositmonths
        {
            get => _systemhouserentdepositmonths;
            set
            {
                _systemhouserentdepositmonths = value;
                OnPropertyChanged();
            }
        }

        private ListModel _selectedHousedepositmonths;
        public ListModel SelectedHousedepositmonths
        {
            get => _selectedHousedepositmonths;
            set
            {
                _selectedHousedepositmonths = value;
                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int rentdepositmonth))
                    {
                        SystempropertyData.Rentdepositmonth = rentdepositmonth;
                    }
                    else
                    {
                        SystempropertyData.Rentdepositmonth = 0;
                    }

                    OnPropertyChanged(nameof(SelectedHousedepositmonths));
                    OnPropertyChanged(nameof(SystempropertyData.Rentdepositmonth));
                }
            }
        }

        public ObservableCollection<ListModel> Systemownerhouse
        {
            get => _systemownerhouse;
            set
            {
                _systemownerhouse = value;
                OnPropertyChanged();
            }
        }

        private ListModel _selectedownerhouse;
        public ListModel Selectedownerhouse
        {
            get => _selectedownerhouse;
            set
            {
                _selectedownerhouse = value;
                // Ensure SystempropertyData is not null
                if (Systemstaffdata != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int propertyhouseid))
                    {
                        Systemstaffdata.Propertyhouseid = propertyhouseid;
                    }
                    else
                    {
                        Systemstaffdata.Propertyhouseid = 0;
                    }

                    OnPropertyChanged(nameof(Selectedownerhouse));
                    OnPropertyChanged(nameof(Systemstaffdata.Propertyhouseid));
                }
            }
        }

        public ObservableCollection<ListModel> Systemhouserentdepositreturndays
        {
            get => _systemhouserentdepositreturndays;
            set
            {
                _systemhouserentdepositreturndays = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedHouserentdepositreturndays;
        public ListModel SelectedHouserentdepositreturndays
        {
            get => _selectedHouserentdepositreturndays;
            set
            {
                _selectedHouserentdepositreturndays = value;
                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int rentdepositreturndays))
                    {
                        SystempropertyData.Rentdepositreturndays = rentdepositreturndays;
                    }
                    else
                    {
                        SystempropertyData.Rentdepositreturndays = 0;
                    }

                    OnPropertyChanged(nameof(SelectedHouserentdepositreturndays));
                    OnPropertyChanged(nameof(SystempropertyData.Rentdepositreturndays));
                }
            }
        }

        public ObservableCollection<ListModel> Systemhouserentingterms
        {
            get => _systemhouserentingterms;
            set
            {
                _systemhouserentingterms = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedHouserentingterms;
        public ListModel SelectedHouserentingterms
        {
            get => _selectedHouserentingterms;
            set
            {
                _selectedHouserentingterms = value;

                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to a string and assign it to Rentingterms
                    if (value != null && !string.IsNullOrEmpty(value.Value))
                    {
                        SystempropertyData.Rentingterms = value.Value;
                    }
                    else
                    {
                        SystempropertyData.Rentingterms = "Month-to-Month";
                    }

                    OnPropertyChanged(nameof(SystempropertyData.Rentingterms));
                }

                OnPropertyChanged(nameof(SelectedHouserentingterms));
                OnPropertyChanged(nameof(IsRentingTermsVisible));
            }
        }

        public ObservableCollection<ListModel> Systemhousevacantnoticeperiod
        {
            get => _systemhousevacantnoticeperiod;
            set
            {
                _systemhousevacantnoticeperiod = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedHousevacantnoticeperiod;
        public ListModel SelectedHousevacantnoticeperiod
        {
            get => _selectedHousevacantnoticeperiod;
            set
            {
                _selectedHousevacantnoticeperiod = value;
                // Ensure SystempropertyData is not null
                if (SystempropertyData != null)
                {
                    // Safely convert the selected value to long and assign it to Countyid
                    if (value != null && int.TryParse(value.Value?.ToString(), out int vacantnoticeperiod))
                    {
                        SystempropertyData.Vacantnoticeperiod = vacantnoticeperiod;
                    }
                    else
                    {
                        SystempropertyData.Vacantnoticeperiod = 0;
                    }

                    OnPropertyChanged(nameof(SelectedHousevacantnoticeperiod));
                    OnPropertyChanged(nameof(SystempropertyData.Vacantnoticeperiod));
                }
            }
        }
        private async Task AddPropertyHouseAsync(long Propertyhouseid)
        {
            IsProcessing = true;
            Systemhouserentingterms = new ObservableCollection<ListModel>
            {
                new ListModel { Value = "Month-to-Month", Text = "Monthly" },
                new ListModel { Value = "Fixedterm", Text = "Fixed Term" },

            };

            Systemhouseentrystatus = new ObservableCollection<ListModel>
            {
                new ListModel { Value = "0", Text = "First Tenants" },
                new ListModel { Value = "1", Text = "Second Tenants" },

            };
            Systemhouserentdueday = new ObservableCollection<ListModel>();
            for (int i = 1; i <= 28; i++)
            {
                string suffix = i switch
                {
                    1 or 21 => "st",
                    2 or 22 => "nd",
                    3 or 23 => "rd",
                    _ => "th"
                };
                Systemhouserentdueday.Add(new ListModel { Value = i.ToString(), Text = $"{i} {suffix} Day" });
            }
            Systemhousedepositmonths = new ObservableCollection<ListModel>();
            for (int i = 1; i <= 6; i++)
            {
                Systemhousedepositmonths.Add(new ListModel { Value = i.ToString(), Text = $"{i} Month{(i > 1 ? "s" : "")}" });
            }
            Systemhouserentdepositreturndays = new ObservableCollection<ListModel>();
            for (int i = 1; i <= 28; i++)
            {
                string suffix = i switch
                {
                    1 or 21 => "st",
                    2 or 22 => "nd",
                    3 or 23 => "rd",
                    _ => "th"
                };
                Systemhouserentdepositreturndays.Add(new ListModel { Value = i.ToString(), Text = $"{i} {suffix} Day" });
            }


            Systemhousevacantnoticeperiod = new ObservableCollection<ListModel>();
            for (int i = 1; i <= 12; i++)
            {
                Systemhousevacantnoticeperiod.Add(new ListModel { Value = i.ToString(), Text = $"{i} Month{(i > 1 ? "s" : "")}" });
            }
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousedetaildatabyid/" + Propertyhouseid, HttpMethod.Get, null);
            if (response != null)
            {
                SystempropertyData = JsonConvert.DeserializeObject<Systemproperty>(response.Data.ToString());
                if (SystempropertyData != null)
                {
                    if (SystempropertyData.Propertyhousestatus > 0)
                    {
                        SelectedHouseentrystatus = Systemhouseentrystatus.FirstOrDefault(x => x.Value == _systempropertyData.Propertyhousestatus.ToString());
                    }
                    if (SystempropertyData.Watertypeid > 0)
                    {
                        SelectedHousewatertype = Systemhousewatertype.FirstOrDefault(x => x.Value == _systempropertyData.Watertypeid.ToString());
                    }
                    if (SystempropertyData.Countyid > 0)
                    {
                        SelectedCounty = Systemcounty.FirstOrDefault(x => x.Value == _systempropertyData.Countyid.ToString());
                    }
                    if (SystempropertyData.Subcountyid > 0)
                    {
                        SelectedSubcounty = Systemsubcounty.FirstOrDefault(x => x.Value == _systempropertyData.Subcountyid.ToString());
                    }
                    if (SystempropertyData.Subcountywardid > 0)
                    {
                        SelectedSubcountyward = Systemsubcountyward.FirstOrDefault(x => x.Value == _systempropertyData.Subcountywardid.ToString());
                    }
                    if (SystempropertyData.Rentdueday > 0)
                    {
                        SelectedHouserentdueday = Systemhouserentdueday.FirstOrDefault(x => x.Value == _systempropertyData.Rentdueday.ToString());
                    }
                    if (SystempropertyData.Rentdepositmonth > 0)
                    {
                        SelectedHousedepositmonths = Systemhousedepositmonths.FirstOrDefault(x => x.Value == _systempropertyData.Rentdepositmonth.ToString());
                    }
                    if (SystempropertyData.Vacantnoticeperiod > 0)
                    {
                        SelectedHousevacantnoticeperiod = Systemhousevacantnoticeperiod.FirstOrDefault(x => x.Value == _systempropertyData.Vacantnoticeperiod.ToString());
                    }
                    if (SystempropertyData.Rentdepositreturndays > 0)
                    {
                        SelectedHouserentdepositreturndays = Systemhouserentdepositreturndays.FirstOrDefault(x => x.Value == _systempropertyData.Rentdepositreturndays.ToString());
                    }
                    if (!string.IsNullOrWhiteSpace(SystempropertyData.Rentingterms))
                    {
                        SelectedHouserentingterms = Systemhouserentingterms.FirstOrDefault(x => x.Value == _systempropertyData.Rentingterms.ToString());
                    }
                }
            }
            var modalPage = new AddSystemPropertyHouseModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }
        private async Task AddAgentPropertyHouseAsync(long Propertyhouseid)
        {
            IsProcessing = true;
            Systemhouserentingterms = new ObservableCollection<ListModel>
            {
                new ListModel { Value = "Month-to-Month", Text = "Monthly" },
                new ListModel { Value = "Fixedterm", Text = "Fixed Term" },

            };

            Systemhouseentrystatus = new ObservableCollection<ListModel>
            {
                new ListModel { Value = "0", Text = "First Tenants" },
                new ListModel { Value = "1", Text = "Second Tenants" },

            };
            Systemhouserentdueday = new ObservableCollection<ListModel>();
            for (int i = 1; i <= 28; i++)
            {
                string suffix = i switch
                {
                    1 or 21 => "st",
                    2 or 22 => "nd",
                    3 or 23 => "rd",
                    _ => "th"
                };
                Systemhouserentdueday.Add(new ListModel { Value = i.ToString(), Text = $"{i} {suffix} Day" });
            }
            Systemhousedepositmonths = new ObservableCollection<ListModel>();
            for (int i = 1; i <= 6; i++)
            {
                Systemhousedepositmonths.Add(new ListModel { Value = i.ToString(), Text = $"{i} Month{(i > 1 ? "s" : "")}" });
            }
            Systemhouserentdepositreturndays = new ObservableCollection<ListModel>();
            for (int i = 1; i <= 28; i++)
            {
                string suffix = i switch
                {
                    1 or 21 => "st",
                    2 or 22 => "nd",
                    3 or 23 => "rd",
                    _ => "th"
                };
                Systemhouserentdepositreturndays.Add(new ListModel { Value = i.ToString(), Text = $"{i} {suffix} Day" });
            }


            Systemhousevacantnoticeperiod = new ObservableCollection<ListModel>();
            for (int i = 1; i <= 12; i++)
            {
                Systemhousevacantnoticeperiod.Add(new ListModel { Value = i.ToString(), Text = $"{i} Month{(i > 1 ? "s" : "")}" });
            }
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousedetaildatabyid/" + Propertyhouseid, HttpMethod.Get, null);
            if (response != null)
            {
                SystempropertyData = JsonConvert.DeserializeObject<Systemproperty>(response.Data.ToString());
                if (SystempropertyData != null)
                {
                    if (SystempropertyData.Propertyhousestatus > 0)
                    {
                        SelectedHouseentrystatus = Systemhouseentrystatus.FirstOrDefault(x => x.Value == _systempropertyData.Propertyhousestatus.ToString());
                    }
                    if (SystempropertyData.Watertypeid > 0)
                    {
                        SelectedHousewatertype = Systemhousewatertype.FirstOrDefault(x => x.Value == _systempropertyData.Watertypeid.ToString());
                    }
                    if (SystempropertyData.Countyid > 0)
                    {
                        SelectedCounty = Systemcounty.FirstOrDefault(x => x.Value == _systempropertyData.Countyid.ToString());
                    }
                    if (SystempropertyData.Subcountyid > 0)
                    {
                        SelectedSubcounty = Systemsubcounty.FirstOrDefault(x => x.Value == _systempropertyData.Subcountyid.ToString());
                    }
                    if (SystempropertyData.Subcountywardid > 0)
                    {
                        SelectedSubcountyward = Systemsubcountyward.FirstOrDefault(x => x.Value == _systempropertyData.Subcountywardid.ToString());
                    }
                    if (SystempropertyData.Rentdueday > 0)
                    {
                        SelectedHouserentdueday = Systemhouserentdueday.FirstOrDefault(x => x.Value == _systempropertyData.Rentdueday.ToString());
                    }
                    if (SystempropertyData.Rentdepositmonth > 0)
                    {
                        SelectedHousedepositmonths = Systemhousedepositmonths.FirstOrDefault(x => x.Value == _systempropertyData.Rentdepositmonth.ToString());
                    }
                    if (SystempropertyData.Vacantnoticeperiod > 0)
                    {
                        SelectedHousevacantnoticeperiod = Systemhousevacantnoticeperiod.FirstOrDefault(x => x.Value == _systempropertyData.Vacantnoticeperiod.ToString());
                    }
                    if (SystempropertyData.Rentdepositreturndays > 0)
                    {
                        SelectedHouserentdepositreturndays = Systemhouserentdepositreturndays.FirstOrDefault(x => x.Value == _systempropertyData.Rentdepositreturndays.ToString());
                    }
                    if (!string.IsNullOrWhiteSpace(SystempropertyData.Rentingterms))
                    {
                        SelectedHouserentingterms = Systemhouserentingterms.FirstOrDefault(x => x.Value == _systempropertyData.Rentingterms.ToString());
                    }
                }
            }
            var modalPage = new AddSystemAgentPropertyHouseModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }

        private async Task AddPropertyHouseCareTakerAsync(long caretakerhouseid)
        {
            IsProcessing = true;
            LoadOwnerHousesByCode();
            if (caretakerhouseid > 0)
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousecaretakerdatabyid/" + caretakerhouseid, HttpMethod.Get, null);
                if (response != null)
                {
                    Systemstaffdata = JsonConvert.DeserializeObject<SystemStaff>(response.Data.ToString());
                    if (Systemstaffdata.Propertyhouseid > 0)
                    {
                        Selectedownerhouse = Systemownerhouse.FirstOrDefault(x => x.Value == _systemstaffdata.Propertyhouseid.ToString());
                    }
                }
                CareTakerFullname = Systemstaffdata.Fullname;
            }
            var modalPage = new AddPropertyHouseCareTakerModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }


        private async Task LoadOwnerHousesByCode()
        {
            try
            {
                var SystemownerhousesResponse = await _serviceProvider.GetSystemDropDownData("/api/General/Getdropdownitembycode?listType=" + ListModelType.Systempropertyhouses + "&code=" + App.UserDetails.Usermodel.Userid, HttpMethod.Get);
                if (SystemownerhousesResponse != null)
                {
                    Systemownerhouse = new ObservableCollection<ListModel>(SystemownerhousesResponse);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async Task LoadDropdownData()
        {
            try
            {
                var SystemcountyResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.SystemCounty, HttpMethod.Get);
                var SystemsubcountyResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.SystemSubCounty, HttpMethod.Get);
                var SystemsubcountywardResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.SystemSubCountyWard, HttpMethod.Get);
                var SystemhousewatertypeResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.Systemhousewatertype, HttpMethod.Get);

                if (SystemcountyResponse != null)
                {
                    Systemcounty = new ObservableCollection<ListModel>(SystemcountyResponse);
                }
                if (SystemsubcountyResponse != null)
                {
                    Systemsubcounty = new ObservableCollection<ListModel>(SystemsubcountyResponse);
                }
                if (SystemsubcountywardResponse != null)
                {
                    Systemsubcountyward = new ObservableCollection<ListModel>(SystemsubcountywardResponse);
                }
                if (SystemhousewatertypeResponse != null)
                {
                    Systemhousewatertype = new ObservableCollection<ListModel>(SystemhousewatertypeResponse);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async Task LoadSubcountyDataCountyCode()
        {
            try
            {
                var SystemsubcountyResponse = await _serviceProvider.GetSystemDropDownData("/api/General/Getdropdownitembycode?listType=" + ListModelType.SystemSubCounty + "&code=" + Convert.ToInt64(SelectedCounty.Value), HttpMethod.Get);
                if (SystemsubcountyResponse != null)
                {
                    Systemsubcounty = new ObservableCollection<ListModel>(SystemsubcountyResponse);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async Task LoadSubcountyWardDataCountyCode()
        {
            try
            {
                var SystemsubcountywardResponse = await _serviceProvider.GetSystemDropDownData("/api/General/Getdropdownitembycode?listType=" + ListModelType.SystemSubCountyWard + "&code=" + Convert.ToInt64(SelectedSubcounty.Value), HttpMethod.Get);
                if (SystemsubcountywardResponse != null)
                {
                    Systemsubcountyward = new ObservableCollection<ListModel>(SystemsubcountywardResponse);
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private async Task LoadItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousedatabyowner/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null && response.Data is List<dynamic> items)
                {
                    Items.Clear();
                    foreach (var item in items)
                    {
                        var product = item.ToObject<Systemproperty>();
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

        private async Task LoadPropertyHouseCaretakerItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousecaretakerdatabyownerid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null && response.Data is List<dynamic> items)
                {
                    PropertyHouseCareTakerItems.Clear();
                    foreach (var item in items)
                    {
                        var Caretaker = item.ToObject<SystemStaff>();
                        PropertyHouseCareTakerItems.Add(Caretaker);
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
        private async Task LoadVacantPropertyHouses()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getallsystempropertyvacanthouses/" + _pageNumber + "/" + 10, HttpMethod.Get, null);
                if (response != null && response.Data is List<dynamic> items)
                {
                    VacantItems.Clear();
                    foreach (var item in items)
                    {
                        var product = item.ToObject<PropertyHouseDetails>();
                        VacantItems.Add(product);
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

        private async Task LoadAgentItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousedatabyagent/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null && response.Data is List<dynamic> items)
                {
                    Items.Clear();
                    foreach (var item in items)
                    {
                        var product = item.ToObject<Systemproperty>();
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
        private async Task ViewHouseDetails(long propertyId)
        {
            IsProcessing = true;
            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousedetaildatabyhouseid/" + propertyId, HttpMethod.Get, null);
                if (response != null && response.Data is List<dynamic> items)
                {
                    Rooms.Clear();
                    foreach (var item in items)
                    {
                        var room = item.ToObject<PropertyHouseDetails>();
                        Rooms.Add(room);
                    }
                    IsDataLoaded = true;
                }
                var detailPage = new PropertyHousesDetailPage(this);
                await Shell.Current.Navigation.PushAsync(detailPage);

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
        private async Task ViewPropertyHouseImagesDetails(long propertyHouseId)
        {
            IsProcessing = true;
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhouseroomimagebyhouseid/" + propertyHouseId, HttpMethod.Get, null);
            if (response != null)
            {
                SystemPropertyHouseImageData = JsonConvert.DeserializeObject<SystemPropertyHouseImage>(response.Data.ToString());
            }
            var modalPage = new SystemPropertyHouseImagesModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }
        public async Task SavePropertyHouseImageasync(string imageUrl)
        {
            IsProcessing = true;


            if (SystemPropertyHouseImageData == null)
            {
                IsProcessing = false;
                return;
            }
            SystemPropertyHouseImageData.Createdby = App.UserDetails.Usermodel.Userid;
            SystemPropertyHouseImageData.Houseorroomimageurl = imageUrl;
            SystemPropertyHouseImageData.Houseorroom = "PropertyHouse";
            SystemPropertyHouseImageData.Datecreated = DateTime.UtcNow;
            try
            {
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseroomimagedata", SystemPropertyHouseImageData);
                if (response.RespStatus == 200 || response.RespStatus == 0)
                {
                    Application.Current.MainPage.Navigation.PopModalAsync();
                }
                else if (response.RespStatus == 1)
                {
                    await Shell.Current.DisplayAlert("Warning", response.RespMessage, "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Sever error occured. Kindly Contact Admin!", "OK");
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
        public bool IsStep1HouseVisible
        {
            get => _isStep1HouseVisible;
            set
            {
                _isStep1HouseVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsStep2HouseVisible
        {
            get => _isStep2HouseVisible;
            set
            {
                _isStep2HouseVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsStep3HouseVisible
        {
            get => _isStep3HouseVisible;
            set
            {
                _isStep3HouseVisible = value;
                OnPropertyChanged();
            }
        }

        public bool IsStep4HouseVisible
        {
            get => _isStep4HouseVisible;
            set
            {
                _isStep4HouseVisible = value;
                OnPropertyChanged();
            }
        }
        public bool IsStep5HouseVisible
        {
            get => _isStep5HouseVisible;
            set
            {
                _isStep5HouseVisible = value;
                OnPropertyChanged();
            }
        }

        private async void HouseNextStep()
        {
            IsLoading = true;


            // Move to the next step
            if (_isStep1HouseVisible)
            {
                if (!ValidateHouseStep1())
                {
                    IsLoading = false;
                    return;
                }
                _isStep1HouseVisible = false;
                _isStep2HouseVisible = true;
            }
            else if (_isStep2HouseVisible)
            {
                if (!ValidateHouseStep2())
                {
                    IsLoading = false;
                    return;
                }
                _isStep2HouseVisible = false;
                _isStep3HouseVisible = true;
            }
            else if (_isStep3HouseVisible)
            {
                _isStep3HouseVisible = false;
                _isStep4HouseVisible = true;
            }
            else if (_isStep4HouseVisible)
            {
                _isStep4HouseVisible = false;
                _isStep5HouseVisible = true;

            }
            IsLoading = false;
            OnPropertyChanged(nameof(IsStep1HouseVisible));
            OnPropertyChanged(nameof(IsStep2HouseVisible));
            OnPropertyChanged(nameof(IsStep3HouseVisible));
            OnPropertyChanged(nameof(IsStep4HouseVisible));
            OnPropertyChanged(nameof(IsStep5HouseVisible));
        }

        private async void HousePreviousStep()
        {
            IsLoading = true;


            // Move to the previous step
            if (_isStep5HouseVisible)
            {
                _isStep5HouseVisible = false;
                _isStep4HouseVisible = true;
            }
            else if (_isStep4HouseVisible)
            {
                _isStep4HouseVisible = false;
                _isStep3HouseVisible = true;
            }
            else if (_isStep3HouseVisible)
            {
                _isStep3HouseVisible = false;
                _isStep2HouseVisible = true;
            }
            else if (_isStep2HouseVisible)
            {
                _isStep2HouseVisible = false;
                _isStep1HouseVisible = true;
            }
            IsLoading = false;

            OnPropertyChanged(nameof(IsStep1HouseVisible));
            OnPropertyChanged(nameof(IsStep2HouseVisible));
            OnPropertyChanged(nameof(IsStep3HouseVisible));
            OnPropertyChanged(nameof(IsStep4HouseVisible));
            OnPropertyChanged(nameof(IsStep5HouseVisible));
        }
        public bool IsStep1HouseRoomVisible
        {
            get => _isStep1HouseRoomVisible;
            set
            {
                _isStep1HouseRoomVisible = value;
                OnPropertyChanged(nameof(IsStep1HouseRoomVisible));
            }
        }

        public bool IsStep2HouseRoomVisible
        {
            get => _isStep2HouseRoomVisible;
            set
            {
                _isStep2HouseRoomVisible = value;
                OnPropertyChanged(nameof(IsStep2HouseRoomVisible));
            }
        }

        public bool IsStep3HouseRoomVisible
        {
            get => _isStep3HouseRoomVisible;
            set
            {
                _isStep3HouseRoomVisible = value;
                OnPropertyChanged(nameof(IsStep3HouseRoomVisible));
            }
        }

        public bool IsStep4HouseRoomVisible
        {
            get => _isStep4HouseRoomVisible;
            set
            {
                _isStep4HouseRoomVisible = value;
                OnPropertyChanged(nameof(IsStep4HouseRoomVisible));
            }
        }
        private async void HouseRoomNextStep()
        {
            IsProcessing = true;
            // Move to the next step
            if (_isStep1HouseRoomVisible)
            {
                if (!ValidateHouseRoomStep1())
                {
                    IsProcessing = false;
                    return;
                }
                _isStep1HouseRoomVisible = false;
                if (HouseroomData.Hashousewatermeter)
                {
                    _isStep2HouseRoomVisible = true;
                    Step2HouseRoomLabel = "Step 2: Sub Meter Reading";
                    Step3HouseRoomLabel = "Step 3: Tenant Details";
                }
                else
                {
                    _isStep2HouseRoomVisible = false;
                    _isStep3HouseRoomVisible = true;
                    Step3HouseRoomLabel = "Step 2: Tenant Details";
                }
            }
            else if (_isStep2HouseRoomVisible)
            {
                if (!ValidateHouseRoomStep2())
                {
                    IsProcessing = false;
                    return;
                }
                _isStep2HouseRoomVisible = false;
                _isStep3HouseRoomVisible = true;
            }
            else if (_isStep3HouseRoomVisible)
            {
                _isStep3HouseRoomVisible = false;
                _isStep4HouseRoomVisible = true;
            }
            IsProcessing = false;
            OnPropertyChanged(nameof(IsStep1HouseRoomVisible));
            OnPropertyChanged(nameof(IsStep2HouseRoomVisible));
            OnPropertyChanged(nameof(IsStep3HouseRoomVisible));
            OnPropertyChanged(nameof(IsStep4HouseRoomVisible));
        }

        private async void HouseRoomPreviousStep()
        {
            IsProcessing = true;


            // Move to the previous step
            if (_isStep4HouseRoomVisible)
            {
                _isStep4HouseRoomVisible = false;
                _isStep3HouseRoomVisible = true;
            }
            else if (_isStep3HouseRoomVisible)
            {
                _isStep3HouseRoomVisible = false;
                if (HouseroomData.Hashousewatermeter)
                {
                    _isStep2HouseRoomVisible = true;
                    Step2HouseRoomLabel = "Step 2: Sub Meter Reading";
                }
                else
                {
                    _isStep2HouseRoomVisible = false;
                    _isStep1HouseRoomVisible = true;
                }
            }
            else if (_isStep2HouseRoomVisible)
            {
                _isStep2HouseRoomVisible = false;
                _isStep1HouseRoomVisible = true;
            }
            IsProcessing = false;

            OnPropertyChanged(nameof(IsStep1HouseRoomVisible));
            OnPropertyChanged(nameof(IsStep2HouseRoomVisible));
            OnPropertyChanged(nameof(IsStep3HouseRoomVisible));
            OnPropertyChanged(nameof(IsStep4HouseRoomVisible));
        }
        public string Step2HouseRoomLabel
        {
            get => _step2HouseRoomLabel;
            set
            {
                _step2HouseRoomLabel = value;
                OnPropertyChanged(nameof(Step2HouseRoomLabel));
            }
        }

        public string Step3HouseRoomLabel
        {
            get => _step3HouseRoomLabel;
            set
            {
                _step3HouseRoomLabel = value;
                OnPropertyChanged(nameof(Step3HouseRoomLabel));
            }
        }

        private bool ValidateHouseRoomStep1()
        {
            bool isValid = true;

            // Validate Property Name
            if (string.IsNullOrWhiteSpace(HouseroomData?.Systempropertyhousesizename))
            {
                PropertyHouseRoomNumberError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseRoomNumberError = null;
            }
            // Validate Property House Water Type
            if (HouseroomData?.Kitchentypeid == 0)
            {
                PropertyHouseKitchenTypeError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseKitchenTypeError = null;
            }
            if (HouseroomData?.Systempropertyhousesizerent == 0)
            {
                PropertyHouseRoomRentError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseRoomRentError = null;
            }
            // Validate Property House County
            if (HouseroomData?.Systempropertyhousesizeid == 0)
            {
                PropertyHouseSizeError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseSizeError = null;
            }
            if (HouseroomData?.Roomoccupant == 0)
            {
                PropertyHouseRoomoccupantError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseRoomoccupantError = null;
            }
            if (string.IsNullOrWhiteSpace(HouseroomData?.Roomoccupantdetail))
            {
                PropertyHouseRoomoccupantdetailError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseRoomoccupantdetailError = null;
            }

            // Update overall IsValid property
            IsProcessing = isValid;

            return isValid;
        }

        private bool ValidateHouseRoomStep2()
        {
            bool isValid = true;

            // Validate Property Name
            if (ClosingMeter < HouseroomData.Openingmeter)
            {
                PropertyHouseRoomClosingMeterError = "Closing Meter Cant be  is required.";
                isValid = false;
            }
            else
            {
                PropertyHouseRoomClosingMeterError = null;
            }
            return isValid;
        }

        private void OnCancelClicked()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
        public async Task SavePropertyHouseAsync()
        {
            IsProcessing = true;


            if (SystempropertyData == null)
            {
                IsProcessing = false;
                return;
            }
            SystempropertyData.Propertyhouseowner = App.UserDetails.Usermodel.Userid;
            SystempropertyData.Createdby = App.UserDetails.Usermodel.Userid;
            SystempropertyData.Modifiedby = App.UserDetails.Usermodel.Userid;
            SystempropertyData.Propertyhouseposter = App.UserDetails.Usermodel.Userid;
            SystempropertyData.Datecreated = DateTime.Now;
            SystempropertyData.Datemodified = DateTime.Now;
            try
            {
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhousedata", SystempropertyData);
                if (response.RespStatus == 200 || response.RespStatus == 0)
                {
                    Application.Current.MainPage.Navigation.PopModalAsync();
                }
                else if (response.RespStatus == 1)
                {
                    await Shell.Current.DisplayAlert("Warning", response.RespMessage, "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Sever error occured. Kindly Contact Admin!", "OK");
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
                    TenantStaffData = JsonConvert.DeserializeObject<Systemtenantdetails>(response.Data.ToString());
                    var modalPage = new StaffAgentOwnerDetailModalPage(this);
                    await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
                }
                else
                {
                    TenantStaffData = new Systemtenantdetails();
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
        private async Task SearchStaff()
        {
            if (IsProcessing || string.IsNullOrWhiteSpace(SearchId))
            {
                PropertyHouseCareTakerError = "Required.";
                return;
            }

            IsProcessing = true;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>($"/api/Account/Getsystemstaffdetaildatabyidnumber/" + SearchId, HttpMethod.Get, null);

                if (response != null)
                {
                    TenantStaffData = JsonConvert.DeserializeObject<Systemtenantdetails>(response.Data.ToString());
                    var modalPage = new StaffCareTakerDetailModalPage(this);
                    await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
                }
                else
                {
                    TenantStaffData = new Systemtenantdetails();
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

        private async Task OnOkCareTakerButtonClicked()
        {
            SearchId = string.Empty;
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/Account/Getsystemstaffprofiledatabyid/" + TenantStaffData.Userid, HttpMethod.Get, null);
            if (response != null)
            {
                Systemstaffdata = JsonConvert.DeserializeObject<SystemStaff>(response.Data.ToString());
                CareTakerFullname = Systemstaffdata.Fullname;
            }
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private void OnCancelCareTakerButtonClicked()
        {
            Systemstaffdata = new SystemStaff();
            SearchId = string.Empty;
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private void OnOkButtonClicked()
        {
            SystempropertyData.Propertyhouseowner = TenantStaffData.Userid;
            SearchId = string.Empty;
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private void OnCancelButtonClicked()
        {
            SystempropertyData.Propertyhouseowner = 0;
            SearchId = string.Empty;
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        public async Task SaveAgentPropertyHouseAsync()
        {
            IsProcessing = true;


            if (SystempropertyData == null)
            {
                IsProcessing = false;
                return;
            }
            SystempropertyData.Isagency = true;
            SystempropertyData.Createdby = App.UserDetails.Usermodel.Userid;
            SystempropertyData.Modifiedby = App.UserDetails.Usermodel.Userid;
            SystempropertyData.Propertyhouseposter = App.UserDetails.Usermodel.Userid;
            SystempropertyData.Datecreated = DateTime.Now;
            SystempropertyData.Datemodified = DateTime.Now;
            try
            {
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhousedata", SystempropertyData);
                if (response.RespStatus == 200 || response.RespStatus == 0)
                {
                    Application.Current.MainPage.Navigation.PopModalAsync();
                }
                else if (response.RespStatus == 1)
                {
                    await Shell.Current.DisplayAlert("Warning", response.RespMessage, "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Sever error occured. Kindly Contact Admin!", "OK");
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

        public async Task SavePropertyHouseCareTakerAsync()
        {
            IsProcessing = true;
            if (Selectedownerhouse == null)
            {
                PropertyOwnerHouseError = "Required.";
                IsProcessing = false;
                return;
            }
            if (Systemstaffdata.Userid == 0)
            {
                PropertyHouseCareTakerError = "Required.";
                IsProcessing = false;
                return;
            }

            if (Systemstaffdata == null)
            {
                IsProcessing = false;
                return;
            }
            Systemstaffdata.Propertyhouseid = Convert.ToInt32(Selectedownerhouse.Value);
            Systemstaffdata.Designation = "Caretaker";
            Systemstaffdata.Parentid = App.UserDetails.Usermodel.Userid;
            Systemstaffdata.Modifiedby = App.UserDetails.Usermodel.Userid;
            Systemstaffdata.Datemodified = DateTime.Now;
            try
            {
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/Account/Registerstaff", Systemstaffdata);
                if (response.RespStatus == 200 || response.RespStatus == 0)
                {
                    Application.Current.MainPage.Navigation.PopModalAsync();
                }
                else if (response.RespStatus == 1)
                {
                    await Shell.Current.DisplayAlert("Warning", response.RespMessage, "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Sever error occured. Kindly Contact Admin!", "OK");
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


        private async Task ViewRoomDetails(long propertyRoomId)
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
                        SelectedKitchentype = Systemkitchentype.FirstOrDefault(x => x.Value == _houseroomData.Kitchentypeid.ToString());
                    }
                    if (sizeResponse != null)
                    {
                        Systempropertyhousesize = new ObservableCollection<ListModel>(sizeResponse);
                        SelectedPropertyhousesize = Systempropertyhousesize.FirstOrDefault(x => x.Value == _houseroomData.Systempropertyhousesizeid.ToString());
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
        private async Task ViewPropertyRoomImagesDetails(long propertyHouseRoomId)
        {
            IsProcessing = true;
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhouseroomimagebyhouseroomid/" + propertyHouseRoomId, HttpMethod.Get, null);
            if (response != null)
            {
                SystemPropertyHouseImageData = JsonConvert.DeserializeObject<SystemPropertyHouseImage>(response.Data.ToString());
            }
            var modalPage = new SystemPropertyHouseRoomImagesModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }
        public async Task SavePropertyHouseRoomImageasync(string imageUrl)
        {
            IsProcessing = true;


            if (SystemPropertyHouseImageData == null)
            {
                IsProcessing = false;
                return;
            }
            SystemPropertyHouseImageData.Createdby = App.UserDetails.Usermodel.Userid;
            SystemPropertyHouseImageData.Houseorroomimageurl = imageUrl;
            SystemPropertyHouseImageData.Houseorroom = "HouseRoom";
            SystemPropertyHouseImageData.Datecreated = DateTime.UtcNow;
            try
            {
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseroomimagedata", SystemPropertyHouseImageData);
                if (response.RespStatus == 200 || response.RespStatus == 0)
                {
                    Application.Current.MainPage.Navigation.PopModalAsync();
                }
                else if (response.RespStatus == 1)
                {
                    await Shell.Current.DisplayAlert("Warning", response.RespMessage, "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Sever error occured. Kindly Contact Admin!", "OK");
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

        private async Task ViewPropertyRoomCheckListDetailAsync(long propertyHouseRoomId)
        {
            IsProcessing = true;

            // Fetch the room fixtures data
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhouseroomfixturesdatabyhouseroomid/" + propertyHouseRoomId, HttpMethod.Get, null);
            if (response != null)
            {
                SystempropertyhouseroomfixturesData = JsonConvert.DeserializeObject<Systempropertyhouseroomfixtures>(response.Data.ToString());
            }

            // Fetch the dropdown data
            var systemPropertyFixturesResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.Systempropertyfixtures, HttpMethod.Get);
            if (systemPropertyFixturesResponse != null)
            {
                // Set SelectedFixture for each RoomFixture
                foreach (var item in SystempropertyhouseroomfixturesData.Roomfixtures)
                {
                    item.Systempropertyfixturesdata = new ObservableCollection<ListModel>(systemPropertyFixturesResponse);
                    item.SelectedFixture = item.Systempropertyfixturesdata.FirstOrDefault(x => x.Value == item.Fixturestatusid.ToString());
                }
            }

            // Navigate to the modal page
            var modalPage = new SystemPropertyHouseRoomCheckListsModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }


        public async Task SavePropertyHouseRoomFixtureasync()
        {
            IsProcessing = true;


            if (SystempropertyhouseroomfixturesData == null)
            {
                IsProcessing = false;
                return;
            }
            try
            {
                foreach (var fixture in SystempropertyhouseroomfixturesData.Roomfixtures)
                {
                    if (fixture.SelectedFixture != null)
                    {
                        // Set the Fixtureid to the selected value
                        fixture.Fixturestatusid = int.Parse(fixture.SelectedFixture.Value);
                    }
                    if (fixture.Fixtureunits > 0 && fixture.Fixturestatusid <= 0)
                    {
                        await Shell.Current.DisplayAlert("Validation Error", "Fixture status is required when units are greater than 0.", "OK");
                        IsProcessing = false;
                        return;
                    }
                }
                SystempropertyhouseroomfixturesData.Datecreated = DateTime.UtcNow;
                SystempropertyhouseroomfixturesData.Createdby = App.UserDetails.Usermodel.Userid;
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseroomfixturedata", SystempropertyhouseroomfixturesData);
                if (response.RespStatus == 200 || response.RespStatus == 0)
                {
                    Application.Current.MainPage.Navigation.PopModalAsync();
                }
                else if (response.RespStatus == 1)
                {
                    await Shell.Current.DisplayAlert("Warning", response.RespMessage, "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Sever error occured. Kindly Contact Admin!", "OK");
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
        private void OnHouseRoomOkClicked()
        {
            HouseroomData.Tenantid = TenantStaffData.Userid;
            SearchId = string.Empty;
            NewTenantStaffData = new Systemtenantdetails
            {
                Fullname = TenantStaffData.Fullname,
                Phonenumber = TenantStaffData.Phonenumber,
                Idnumber = TenantStaffData.Idnumber,
            };
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
        private void OnHouseRoomCancelClicked()
        {
            Tenantid = 0;
            NewTenantStaffData = new Systemtenantdetails
            {
                Fullname = "No Tenant selected",
                Phonenumber = "No Tenant selected",
                Idnumber = 0,
            };
            SearchId = string.Empty;
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private async Task SaveHouseRoomDetailsAsync()
        {
            IsProcessing = true;
            if (HouseroomData.Tenantid == 0)
            {
                PropertyHouseRoomTenantidError = "New Tenant is required.";
                return;
            }

            if (!ValidateHouseRoomStep1())
            {
                IsProcessing = false;
                return;
            }
            if (HouseroomData == null)
            {
                IsProcessing = false;
                return;
            }
            try
            {
                HouseroomData.Createdby = App.UserDetails.Usermodel.Userid;
                HouseroomData.Datecreated = DateTime.UtcNow;

                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Registerpropertyhouseroomdata", HttpMethod.Post, HouseroomData);
                if (response.StatusCode == 200)
                {
                    Application.Current.MainPage.Navigation.PopModalAsync();
                }
                else if (response.StatusCode == 1)
                {
                    await Shell.Current.DisplayAlert("Warning", "Something went wrong. Contact Admin!", "OK");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Error", "Sever error occured. Kindly Contact Admin!", "OK");
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



        private bool ValidateHouseStep1()
        {
            bool isValid = true;

            // Validate Property Name
            if (string.IsNullOrWhiteSpace(SystempropertyData.Propertyhousename))
            {
                PropertyHouseNameError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseNameError = null;
            }

            if (SystempropertyData.Monthlycollection == 0 || SystempropertyData.Monthlycollection < 20000)
            {
                PropertyHouseMonthlycollectionError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseMonthlycollectionError = null;
            }

            // Validate Street or Landmark
            if (string.IsNullOrWhiteSpace(SystempropertyData?.Streetorlandmark))
            {
                StreetOrLandmarkError = "Required.";
                isValid = false;
            }
            else
            {
                StreetOrLandmarkError = null;
            }

            // Validate Contact Details
            if (string.IsNullOrWhiteSpace(SystempropertyData?.Contactdetails))
            {
                ContactDetailsError = "Required.";
                isValid = false;
            }
            else
            {
                ContactDetailsError = null;
            }

            // Validate Property House Status
            if (SelectedHouseentrystatus == null)
            {
                PropertyHouseStatusError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseStatusError = null;
            }
            // Validate Property House Water Type
            if (SelectedHousewatertype == null)
            {
                PropertyHouseWaterTypeError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseWaterTypeError = null;
            }
            // Validate Property House County
            if (SelectedCounty == null)
            {
                PropertyHouseCountyError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseCountyError = null;
            }
            // Validate Property House Sub County
            if (SelectedSubcounty == null)
            {
                PropertyHouseSubcountyError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseSubcountyError = null;
            }
            // Validate Property House sub County Ward
            if (SelectedSubcountyward == null)
            {
                PropertyHouseSubcountyWardError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseSubcountyWardError = null;
            }
            // Validate Property House Rent Deposit
            if (SelectedHouserentdueday == null)
            {
                PropertyHouseRentDueDayError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseRentDueDayError = null;
            }
            // Validate Property House Rent Deposit Months
            if (SelectedHousedepositmonths == null)
            {
                PropertyHouseRentDepositMonthsError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseRentDepositMonthsError = null;
            }
            // Validate Property House Vacation Period
            if (SelectedHousevacantnoticeperiod == null)
            {
                PropertyHouseRentVacationPeriodMonthsError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseRentVacationPeriodMonthsError = null;
            }
            if (SelectedHouserentdepositreturndays == null)
            {
                PropertyHouseRentDepositReturnDaysError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseRentDepositReturnDaysError = null;
            }
            if (SystempropertyData.Allowpets)
            {
                if (SystempropertyData.Numberofpets < 0)
                {
                    PropertyHouseNumberofpetsError = "Required.";
                }
                else
                {
                    PropertyHouseNumberofpetsError = null;
                }
                if (SystempropertyData.Petdeposit < 0)
                {
                    PropertyHousePetdepositError = "Required.";
                }
                else
                {
                    PropertyHousePetdepositError = null;
                }
                if (string.IsNullOrWhiteSpace(SystempropertyData.Petparticulars))
                {
                    PropertyHousePetparticularsError = "Required.";
                }
                else
                {
                    PropertyHousePetparticularsError = null;
                }
                isValid = false;
            }
            else
            {
                PropertyHouseNumberofpetsError = null;
                PropertyHousePetdepositError = null;
                PropertyHousePetparticularsError = null;
            }
            if (SelectedHouserentingterms == null)
            {
                PropertyHouseRentingTermsError = "Required.";
                isValid = false;
            }
            else
            {
                if (SelectedHouserentingterms.Value == "Fixedterm")
                {
                    if (SystempropertyData.Enddate == null || SystempropertyData.Enddate >= DateTime.Now.Date)
                    {
                        PropertyHouseRentingTermsEnddateError = "Required.";
                    }
                    else
                    {
                        PropertyHouseRentingTermsEnddateError = null;
                    }
                }
                PropertyHouseRentingTermsError = null;
            }
            // Update overall IsValid property
            IsValid = isValid;

            return isValid;
        }

        private bool ValidateHouseStep2()
        {
            bool isValid = true;

            // Validate Property Name
            if (!SystempropertyData.Propertyhousesize.Any(x => x.Systempropertyhousesizeunits > 0))
            {
                PropertyHouseRoomSizeError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseRoomSizeError = null;
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
