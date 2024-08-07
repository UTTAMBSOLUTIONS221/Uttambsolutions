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

        private bool _isStep1Visible;
        private bool _isStep2Visible;
        private bool _isStep3Visible;

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
            CloseCommand = new Command(() => Close());
            NextCommand = new Command(NextStep);
            PreviousCommand = new Command(PreviousStep);
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

        public bool ContinueWithoutTenant
        {
            get => _continueWithoutTenant;
            set
            {
                _continueWithoutTenant = value;
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
            // Implement search logic based on SearchId
        }

        private void Close()
        {
            // Implement close logic, if needed
        }

        private void NextStep()
        {
            if (IsStep1Visible)
            {
                IsStep1Visible = false;
                IsStep2Visible = true;
            }
            else if (IsStep2Visible)
            {
                IsStep2Visible = false;
                IsStep3Visible = true;
            }
        }

        private void PreviousStep()
        {
            if (IsStep3Visible)
            {
                IsStep3Visible = false;
                IsStep2Visible = true;
            }
            else if (IsStep2Visible)
            {
                IsStep2Visible = false;
                IsStep1Visible = true;
            }
        }
    }
}
