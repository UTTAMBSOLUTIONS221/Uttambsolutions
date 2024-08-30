using DBL.Entities;
using DBL.Enum;
using DBL.Models;
using Firebase.Storage;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;
using Maqaoplus.Views.PropertyHouse.Modal;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Font = iTextSharp.text.Font;

namespace Maqaoplus.ViewModels.PropertyHouse
{
    public class PropertyHouseViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public event PropertyChangedEventHandler PropertyChanged;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        public string AgreementText { get; set; }
        public ObservableCollection<Systemproperty> Items { get; }
        private Systemproperty _systempropertyData;
        private OwnerTenantAgreementDetailData _ownerTenantAgreementDetailData;
        private SystemPropertyHouseImage _systemPropertyHouseImageData;

        private bool _isStep1Visible;
        private bool _isStep2Visible;
        private bool _isStep3Visible;
        private bool _isStep4Visible;
        private bool _isStep5Visible;

        public ICommand AddPropertyHouseCommand { get; }
        public ICommand EditPropertyHouseCommand { get; }
        public ICommand LoadItemsCommand { get; }
        public ICommand ViewDetailsCommand { get; }
        public ICommand ViewPropertyAgreementCommand { get; }
        public ICommand ViewPropertyHouseImageCommand { get; }
        public ICommand NextCommand { get; }
        public ICommand PreviousCommand { get; }
        public ICommand OnCancelClickedCommand { get; }
        public ICommand SavePropertyHouseCommand { get; }


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
                        _systempropertyData.PropertyChanged -= OnSystempropertyDataChanged;
                    }

                    _systempropertyData = value;

                    if (_systempropertyData != null)
                    {
                        // Subscribe to new instance's property change events
                        _systempropertyData.PropertyChanged += OnSystempropertyDataChanged;
                    }

                    OnPropertyChanged(nameof(SystempropertyData));
                    OnPropertyChanged(nameof(IsPetsAllowedVisible));
                }
            }
        }
        public bool IsPetsAllowedVisible => SystempropertyData?.Allowpets ?? false;
        private void OnSystempropertyDataChanged(object sender, PropertyChangedEventArgs e)
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
                // Check if SystempropertyData is not null and if Rentingterms is a fixed term
                return SystempropertyData != null && IsFixedTerm(SystempropertyData.Rentingterms);
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

        public SystemPropertyHouseImage SystemPropertyHouseImageData
        {
            get => _systemPropertyHouseImageData;
            set
            {
                _systemPropertyHouseImageData = value;
                OnPropertyChanged();
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

        public ObservableCollection<Systempropertyhousesize> PropertyHouseSizes { get; set; } = new ObservableCollection<Systempropertyhousesize>();
        public ObservableCollection<Systempropertyhousedepositfees> PropertyHouseDepositFees { get; set; } = new ObservableCollection<Systempropertyhousedepositfees>();
        public ObservableCollection<Systempropertyhousebenefits> PropertyHouseBenefits { get; set; } = new ObservableCollection<Systempropertyhousebenefits>();

        // Parameterless constructor for XAML support
        public PropertyHouseViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Items = new ObservableCollection<Systemproperty>();
            AddPropertyHouseCommand = new Command<Systemproperty>(async (property) => { var propertyId = property?.Propertyhouseid ?? 0; await AddPropertyHouseAsync(propertyId); });
            LoadItemsCommand = new Command(async () => await LoadItems());
            ViewDetailsCommand = new Command<Systemproperty>(async (property) => await ViewDetails(property.Propertyhouseid));
            ViewPropertyAgreementCommand = new Command<Systemproperty>(async (property) => await ViewPropertyAgreementDetails(property.Propertyhouseid, property.Propertyhouseowner));
            ViewPropertyHouseImageCommand = new Command<Systemproperty>(async (property) => await ViewPropertyHouseImagesDetails(property.Propertyhouseid));
            NextCommand = new Command(NextStep);
            PreviousCommand = new Command(PreviousStep);
            OnCancelClickedCommand = new Command(OnCancelClicked);
            SavePropertyHouseCommand = new Command(async () => await SavePropertyHouseAsync());


            // Initialize steps
            _isStep1Visible = true;
            _isStep2Visible = false;
            _isStep3Visible = false;
            _isStep4Visible = false;
            _isStep5Visible = false;
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
                OnPropertyChanged(nameof(SelectedCounty));
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
                OnPropertyChanged(nameof(SelectedSubcounty));
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
                OnPropertyChanged(nameof(SelectedSubcountyward));
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
                OnPropertyChanged(nameof(SelectedHouseentrystatus));
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
                OnPropertyChanged(nameof(SelectedHousewatertype));
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
                OnPropertyChanged(nameof(SelectedHouserentdueday));
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
                OnPropertyChanged(nameof(SelectedHousedepositmonths));
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
                OnPropertyChanged(nameof(SelectedHouserentdepositreturndays));
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
                OnPropertyChanged(nameof(SelectedHousevacantnoticeperiod));
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
            LoadDropdownData();
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousedetaildatabyid/" + Propertyhouseid, HttpMethod.Get, null);
            if (response != null)
            {
                SystempropertyData = JsonConvert.DeserializeObject<Systemproperty>(response.Data.ToString());
            }
            var modalPage = new AddSystemPropertyHouseModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
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
        private async Task ViewDetails(long propertyId)
        {
            IsProcessing = true;
            try
            {
                var encodedPropertyId = Uri.EscapeDataString(propertyId.ToString());
                await Shell.Current.GoToAsync($"PropertyHousesDetailPage?PropertyId={encodedPropertyId}");
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
        private async Task ViewPropertyAgreementDetails(long propertyId, long Ownerid)
        {
            IsProcessing = true;
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhouseagreementdetaildatabypropertyidandownerid/" + propertyId + "/" + Ownerid, HttpMethod.Get, null);
            if (response != null)
            {
                OwnerTenantAgreementDetailData = JsonConvert.DeserializeObject<OwnerTenantAgreementDetailData>(response.Data.ToString());
            }
            var modalPage = new SystemPropertyHouseAgreementModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }

        public async Task AgreeToPropertyHouseAgreementasync(string imageUrl)
        {
            IsProcessing = true;

            await Task.Delay(500);
            if (OwnerTenantAgreementDetailData == null)
            {
                IsProcessing = false;
                return;
            }
            OwnerTenantAgreementDetailData.Propertyhouseowner = App.UserDetails.Usermodel.Userid;
            OwnerTenantAgreementDetailData.Signatureimageurl = imageUrl;
            OwnerTenantAgreementDetailData.Ownerortenant = "Owner";
            OwnerTenantAgreementDetailData.Agreementname = OwnerTenantAgreementDetailData.Fullname + " Property " + OwnerTenantAgreementDetailData.Propertyhousename + " Owner Agreement";
            OwnerTenantAgreementDetailData.Datecreated = DateTime.UtcNow;
            try
            {
                var response = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseagreementdata", OwnerTenantAgreementDetailData);
                if (response.RespStatus == 200 || response.RespStatus == 0)
                {
                    OwnerTenantAgreementDetailData.OwnerSignatureimageurl = response.Data2;
                    OwnerTenantAgreementDetailData.Agreementdetailpdfurl = await GenerateAndUploadAgreementPdfAsync();
                    OwnerTenantAgreementDetailData.Agreementid = Convert.ToInt64(response.Data1);
                    var responseAfter = await _serviceProvider.CallCustomUnAuthWebApi("/api/PropertyHouse/Registersystempropertyhouseagreementdata", OwnerTenantAgreementDetailData);
                    if (responseAfter.RespStatus == 200 || responseAfter.RespStatus == 0)
                    {
                        Application.Current.MainPage.Navigation.PopModalAsync();
                    }
                    else if (responseAfter.RespStatus == 1)
                    {
                        await Shell.Current.DisplayAlert("Warning", responseAfter.RespMessage, "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Error", "Sever error occured. Kindly Contact Admin!", "OK");

                    }
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

            await Task.Delay(500);
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
        public bool IsStep5Visible
        {
            get => _isStep5Visible;
            set
            {
                _isStep5Visible = value;
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
            else if (_isStep4Visible)
            {
                _isStep4Visible = false;
                _isStep5Visible = true;

            }
            IsLoading = false;
            OnPropertyChanged(nameof(IsStep1Visible));
            OnPropertyChanged(nameof(IsStep2Visible));
            OnPropertyChanged(nameof(IsStep3Visible));
            OnPropertyChanged(nameof(IsStep4Visible));
            OnPropertyChanged(nameof(IsStep5Visible));
        }

        private async void PreviousStep()
        {
            IsLoading = true;

            await Task.Delay(500);
            // Move to the previous step
            if (_isStep5Visible)
            {
                _isStep5Visible = false;
                _isStep4Visible = true;
            }
            else if (_isStep4Visible)
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
            OnPropertyChanged(nameof(IsStep5Visible));
        }
        private void OnCancelClicked()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }
        public async Task SavePropertyHouseAsync()
        {
            IsProcessing = true;

            await Task.Delay(500);
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

        private bool ValidateStep1()
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
            if (SelectedHouserentingterms == null)
            {
                PropertyHouseRentingTermsError = "Required.";
                isValid = false;
            }
            else
            {
                PropertyHouseRentingTermsError = null;
            }
            // Update overall IsValid property
            IsValid = isValid;

            return isValid;
        }

        public async Task<string> GenerateAndUploadAgreementPdfAsync()
        {
            if (OwnerTenantAgreementDetailData == null)
                return null;

            using (var memoryStream = new MemoryStream())
            {
                using (var document = new Document(PageSize.A4, 50, 50, 50, 50))
                {
                    PdfWriter.GetInstance(document, memoryStream);
                    document.Open();

                    // Define fonts
                    var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 20);
                    var sectionHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                    var regularFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
                    var smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);

                    // Title
                    var titleParagraph = new Paragraph("RENTAL MANAGEMENT SYSTEM AGREEMENT", titleFont);
                    titleParagraph.Alignment = iTextSharp.text.Element.ALIGN_CENTER;
                    document.Add(titleParagraph);
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing

                    // Add a line separator
                    var lineSeparator = new LineSeparator(1, 100, BaseColor.Black, iTextSharp.text.Element.ALIGN_CENTER, -2);
                    document.Add(lineSeparator);
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing

                    // Date
                    document.Add(new Paragraph($"Date: {OwnerTenantAgreementDetailData.OwnerDatecreated:yyyy-MM-dd}", smallFont));
                    document.Add(new Paragraph(" ", smallFont)); // Add spacing

                    // Property Owner Details
                    document.Add(new Paragraph("Property Owner Details", sectionHeaderFont));
                    document.Add(new Paragraph($"Property: {OwnerTenantAgreementDetailData.Propertyhousename}", regularFont));
                    document.Add(new Paragraph($"Name: {OwnerTenantAgreementDetailData.Fullname}", regularFont));
                    document.Add(new Paragraph($"Address: {OwnerTenantAgreementDetailData.Countyname}, {OwnerTenantAgreementDetailData.Subcountyname}, {OwnerTenantAgreementDetailData.Subcountywardname}", regularFont));
                    document.Add(new Paragraph($"Phone: {OwnerTenantAgreementDetailData.Phonenumber}", regularFont));
                    document.Add(new Paragraph($"Email: {OwnerTenantAgreementDetailData.Emailaddress}", regularFont));
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing

                    // Rental Management System Provider
                    document.Add(new Paragraph("Rental Management System Provider", sectionHeaderFont));
                    document.Add(new Paragraph("Name: UTTAMB SOLUTIONS LIMITED", regularFont));
                    document.Add(new Paragraph("Address: Nairobi, Kenya", regularFont));
                    document.Add(new Paragraph("Phone: 0717850720", regularFont));
                    document.Add(new Paragraph("Email: support@uttambsolutions.com", regularFont));
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing

                    // Agreement Sections
                    AddAgreementSection(document, "1. PURPOSE OF THE AGREEMENT", sectionHeaderFont, regularFont, $"The purpose of this Agreement is to outline the terms and conditions under which UTTAMB SOLUTIONS LIMITED (hereinafter referred to as the Management System Provider) will provide rental management services to {OwnerTenantAgreementDetailData.Fullname} (hereinafter referred to as the Property Owner) for the property located at {OwnerTenantAgreementDetailData.Countyname}, {OwnerTenantAgreementDetailData.Subcountyname}, {OwnerTenantAgreementDetailData.Subcountywardname} (hereinafter referred to as the Property).");
                    AddAgreementSection(document, "2. SERVICES PROVIDED", sectionHeaderFont, regularFont, "- Advertising and Marketing: Listing the Property on various platforms to attract potential tenants.\n- Tenant Screening: Conducting background checks and verifying tenant credentials.\n- Rent Collection: Facilitating the collection of rent payments from tenants.\n- Property Maintenance: Coordinating with contractors for repairs and regular maintenance of the Property.\n- Reporting: Providing regular reports on the status of the Property, rent collection, and any issues that arise.");
                    AddAgreementSection(document, "3. FEES AND PAYMENTS", sectionHeaderFont, regularFont, $"- Service Fee: The Property Owner agrees to pay the Management System Provider a service fee of 1% of the monthly rent collected.\n- Subscription Payment: The Property Owner agrees to pay a subscription fee for the services rendered by the Management System Provider. The subscription fee shall be paid monthly to the following bank account:\n\n  Bank Name: FAMILY BANK\n  Pay Bill: 222111\n  Account Number: 2340982\n\n- Payment Terms: The subscription fee is due on the 10th day of each month.\n- Additional Costs: Any costs related to property maintenance, legal fees, or other services not covered under this Agreement will be billed separately with the Property Owner's prior approval.");
                    AddAgreementSection(document, "4. PROPERTY OWNER RESPONSIBILITIES", sectionHeaderFont, regularFont, "- Property Upkeep: The Property Owner agrees to maintain the Property in a condition suitable for rental.\n- Insurance: The Property Owner is responsible for obtaining and maintaining appropriate insurance coverage for the Property.\n- Legal Compliance: The Property Owner agrees to comply with all local, county, and national laws relating to the rental and maintenance of the Property.");
                    AddAgreementSection(document, "5. DATA PROTECTION AND PRIVACY", sectionHeaderFont, regularFont, "- Compliance with Data Protection Act, 2019: The Management System Provider shall ensure that all personal data collected, processed, and stored as part of the rental management services is handled in accordance with the Data Protection Act, 2019 of Kenya.\n- Data Security: Both parties agree to implement appropriate technical and organizational measures to protect personal data against unauthorized or unlawful processing, accidental loss, destruction, or damage.");
                    AddAgreementSection(document, "6. TERM AND TERMINATION", sectionHeaderFont, regularFont, $"- Term: This Agreement will begin on {OwnerTenantAgreementDetailData.OwnerDatecreated:yyyy-MM-dd} and will continue until terminated by either party.\n- Termination: Either party may terminate this Agreement with 14 days' written notice. Upon termination, the Property Owner is responsible for any outstanding fees and obligations under this Agreement.");
                    AddAgreementSection(document, "7. INDEMNIFICATION", sectionHeaderFont, regularFont, "The Property Owner agrees to indemnify and hold harmless the Management System Provider from any claims, liabilities, or damages arising out of the management of the Property, except in cases of gross negligence or willful misconduct by the Management System Provider.");
                    AddAgreementSection(document, "8. GOVERNING LAW", sectionHeaderFont, regularFont, "This Agreement shall be governed by and construed in accordance with the laws of Kenya.");
                    AddAgreementSection(document, "9. ENTIRE AGREEMENT", sectionHeaderFont, regularFont, "This Agreement constitutes the entire agreement between the parties with respect to its subject matter and supersedes all prior agreements and understandings, whether written or oral.");

                    // Signatures
                    document.Add(new Paragraph("AGREED AND ACCEPTED", sectionHeaderFont));
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing

                    // Create a table with 2 columns
                    var table = new PdfPTable(2)
                    {
                        WidthPercentage = 100
                    };

                    // Set column widths (adjust as necessary)
                    table.SetWidths(new float[] { 1f, 1f });
                    // Add Management System Provider signature
                    AddSignatureToTable(table, "Management System Provider", "Francis Kingori-Director \n Uttamb Solutions Limited", "https://firebasestorage.googleapis.com/v0/b/uttambsolutions-4ec2a.appspot.com/o/UttambSolutionsPrivate%2Fmysignature.jpg?alt=media&token=d970f2d8-f4bd-4a30-b47e-12d9f8d1edc9", OwnerTenantAgreementDetailData.OwnerDatecreated);
                    // Add Property Owner signature
                    AddSignatureToTable(table, "Property Owner", OwnerTenantAgreementDetailData.Fullname, OwnerTenantAgreementDetailData.OwnerSignatureimageurl, OwnerTenantAgreementDetailData.OwnerDatecreated);


                    // Add the table to the document
                    document.Add(table);

                    document.Add(new Paragraph(" ", regularFont)); // Add spacing
                    document.Add(new Paragraph(" ", regularFont)); // Add spacing
                    document.Add(new Paragraph("This Agreement constitutes the entire understanding between the parties and supersedes all prior agreements, whether written or oral, relating to the subject matter herein.", regularFont));

                    // Close the document
                    document.Close();
                }

                // Convert memory stream to byte array
                var pdfBytes = memoryStream.ToArray();

                // Upload to Firebase Storage
                var storage = new FirebaseStorage("uttambsolutions-4ec2a.appspot.com");
                var stream = new MemoryStream(pdfBytes);

                // Sanitize the file name to avoid issues with special characters
                string sanitizedFullName = OwnerTenantAgreementDetailData.Fullname.Replace(" ", "_").Replace("/", "_").Replace("\\", "_");
                string sanitizedPropertyName = OwnerTenantAgreementDetailData.Propertyhousename.Replace(" ", "_").Replace("/", "_").Replace("\\", "_");
                var fileName = $"{sanitizedFullName}_{sanitizedPropertyName}_Owner_Agreement.pdf";
                var uploadTask = storage.Child("maqaoplus").Child("agreements").Child(fileName).PutAsync(stream);
                var downloadUrl = await uploadTask;
                return downloadUrl;
            }
        }

        private void AddAgreementSection(Document document, string title, Font titleFont, Font contentFont, string content)
        {
            document.Add(new Paragraph(title, titleFont));
            document.Add(new Paragraph(content, contentFont));
            document.Add(new Paragraph(" ", contentFont)); // Add spacing
        }

        private void AddSignatureToTable(PdfPTable table, string role, string name, string signatureImageUrl, DateTime date)
        {
            // Create a cell to hold the image and text
            var cell = new PdfPCell
            {
                Border = Rectangle.NO_BORDER,
                Padding = 5,
                VerticalAlignment = iTextSharp.text.Element.ALIGN_TOP // Aligns the content to the top
            };

            // Add the image
            if (!string.IsNullOrEmpty(signatureImageUrl))
            {
                try
                {
                    var signatureImage = iTextSharp.text.Image.GetInstance(signatureImageUrl);
                    signatureImage.ScaleToFit(150, 75); // Adjust size as needed

                    // Create a paragraph for the image and text
                    var imageParagraph = new Paragraph
                {
                    new Chunk(signatureImage, 0, 0),
                    new Chunk($"\n{name}", FontFactory.GetFont(FontFactory.HELVETICA, 12)),
                    new Chunk($"\n{role}", FontFactory.GetFont(FontFactory.HELVETICA, 12)),
                    new Chunk($"\nDate: {date:yyyy-MM-dd}", FontFactory.GetFont(FontFactory.HELVETICA, 12))
                };

                    cell.AddElement(imageParagraph);
                }
                catch (Exception ex)
                {
                    // Handle image loading exceptions
                    cell.AddElement(new Paragraph($"Error loading image: {ex.Message}", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
                }
            }
            else
            {
                cell.AddElement(new Paragraph($"No signature available for {role}.", FontFactory.GetFont(FontFactory.HELVETICA, 12)));
            }

            // Add cell to table
            table.AddCell(cell);
        }


        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
