using DBL.Enum;
using DBL.Models;
using Maqaoplus.Views;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.Reports
{
    public class SystemReportsViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        private ObservableCollection<ListModel> _systempropertyhouses;

        private bool _isLoading;
        private bool _isProcessing;
        private bool _isDataLoaded;
        private bool _isValid;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged(nameof(IsProcessing));
                ((Command)LoadReportModalCommand).ChangeCanExecute();
            }
        }

        public bool IsDataLoaded
        {
            get => _isDataLoaded;
            set
            {
                _isDataLoaded = value;
                OnPropertyChanged(nameof(IsDataLoaded));
            }
        }

        public bool IsValid
        {
            get => _isValid;
            private set
            {
                if (_isValid != value)
                {
                    _isValid = value;
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }
        private ObservableCollection<ObservableCollection<ListModel>> _dropdownCollections;
        public ObservableCollection<ObservableCollection<ListModel>> DropdownCollections
        {
            get => _dropdownCollections;
            set
            {
                _dropdownCollections = value;
                OnPropertyChanged(nameof(DropdownCollections));
            }
        }

        private ObservableCollection<ListModel> _selectedSystempropertyhouses;
        public ObservableCollection<ListModel> SelectedSystempropertyhouses
        {
            get => _selectedSystempropertyhouses;
            set
            {
                _selectedSystempropertyhouses = value;
                OnPropertyChanged(nameof(SelectedSystempropertyhouses));
            }
        }


        public ICommand LoadReportModalCommand { get; }

        public SystemReportsViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadReportModalCommand = new Command<string>(async (reportType) => await LoadReportModalAsync(reportType));
            DropdownCollections = new ObservableCollection<ObservableCollection<ListModel>>();
            SelectedSystempropertyhouses = new ObservableCollection<ListModel>();
        }


        private async Task LoadReportModalAsync(string reportType)
        {
            if (IsProcessing)
                return;

            IsLoading = true;

            try
            {
                // Clear previous items
                DropdownCollections.Clear();

                SelectedSystempropertyhouses.Clear();
                // Load data based on report type
                switch (reportType)
                {
                    case "propertyhousesandrooms":
                        // Load sales report data into dropdown
                        var SystempropertyhousesResponse = await _serviceProvider.GetSystemDropDownData("/api/General/Getdropdownitembycode?listType=" + ListModelType.Systempropertyhouses + "&code=" + App.UserDetails.Usermodel.Userid, HttpMethod.Get);
                        if (SystempropertyhousesResponse != null)
                        {
                            var collection = new ObservableCollection<ListModel>(SystempropertyhousesResponse);
                            DropdownCollections.Add(collection);
                            SelectedSystempropertyhouses.Add(collection.FirstOrDefault());
                        }
                        break;

                    case "anotherreport":

                        break;

                }

                IsDataLoaded = true;

                // Navigate to the modal with the customer data
                var modalPage = new SystemReportDetailModalPage(
                    new Command(OnOkClicked),
                    new Command(OnCancelClicked)
                );
                await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
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
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        private void OnCancelClicked()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
