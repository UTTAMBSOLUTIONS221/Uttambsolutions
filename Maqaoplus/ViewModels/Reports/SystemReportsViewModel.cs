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

        // Collection for dropdown items
        private ObservableCollection<string> _dropdownItems;
        public ObservableCollection<string> DropdownItems
        {
            get => _dropdownItems;
            set
            {
                _dropdownItems = value;
                OnPropertyChanged(nameof(DropdownItems));
            }
        }

        public ICommand LoadReportModalCommand { get; }

        public SystemReportsViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadReportModalCommand = new Command<string>(LoadReportModalAsync);
            DropdownItems = new ObservableCollection<string>();
        }

        public ObservableCollection<ListModel> Systempropertyhouses
        {
            get => _systempropertyhouses;
            set
            {
                _systempropertyhouses = value;
                OnPropertyChanged(nameof(Systempropertyhouses));
            }
        }
        private ListModel _selectedSystempropertyhouses;
        public ListModel SelectedSystempropertyhouses
        {
            get => _selectedSystempropertyhouses;
            set
            {
                _selectedSystempropertyhouses = value;
            }
        }
        private async Task LoadReportModalAsync(string reportType)
        {
            if (IsProcessing)
                return;

            IsLoading = true;

            try
            {
                // Clear previous items
                DropdownItems.Clear();

                // Load data based on report type
                switch (reportType)
                {
                    case "salesreport":
                        // Load sales report data into dropdown
                        var SystempropertyhousesResponse = await _serviceProvider.GetSystemDropDownData("/api/General?listType=" + ListModelType.Systempropertyhouses, HttpMethod.Get);
                        if (SystempropertyhousesResponse != null)
                        {
                            Systempropertyhouses = new ObservableCollection<ListModel>(SystempropertyhousesResponse);
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
