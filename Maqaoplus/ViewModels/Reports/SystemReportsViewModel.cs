using Maqaoplus.Views;
using System.ComponentModel;
using System.Windows.Input;
namespace Maqaoplus.ViewModels.Reports
{
    public class SystemReportsViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
        private bool _isProcessing;
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
        private bool _isDataLoaded;
        public bool IsDataLoaded
        {
            get => _isDataLoaded;
            set
            {
                _isDataLoaded = value;
                OnPropertyChanged(nameof(IsDataLoaded));
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
                    OnPropertyChanged(nameof(IsValid));
                }
            }
        }

        public ICommand LoadReportModalCommand { get; }


        public SystemReportsViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadReportModalCommand = new Command(async () => await LoadReportModalAsync());
        }
        private async Task LoadReportModalAsync()
        {
            if (IsProcessing)
                return;

            IsLoading = true;

            try
            {
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
