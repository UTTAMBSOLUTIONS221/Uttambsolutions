using DBL.Models.Dashboards;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.Dashboards
{
    public class SummaryDashBoardViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        private PropertyHouseSummary _dashBoardSummaryData;

        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand LoadItemsCommand { get; }

        public PropertyHouseSummary DashBoardSummaryData
        {
            get => _dashBoardSummaryData ?? new PropertyHouseSummary
            {
                Propertyhouseunits = 0,
                Systempropertyoccupiedroom = 0,
                Systempropertyvacantroom = 0,
                Rentarrears = 0,
                Uncollectedpayments = 0,
                Consumedmeters = 0,
                Propertybysummary = new List<PropertySummary>()
            };
            set
            {
                _dashBoardSummaryData = value;
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

        public SummaryDashBoardViewModel(Services.ServiceProvider serviceProvider)
        {
            DashBoardSummaryData = new PropertyHouseSummary
            {
                Propertyhouseunits = 0,
                Systempropertyoccupiedroom = 0,
                Systempropertyvacantroom = 0,
                Rentarrears = 0,
                Uncollectedpayments = 0,
                Consumedmeters = 0,
                Propertybysummary = new List<PropertySummary>()
            };

            _serviceProvider = serviceProvider;
            LoadItemsCommand = new Command(async () => await LoadItems());
        }

        private async Task LoadItems()
        {
            IsProcessing = true;
            IsDataLoaded = false;

            try
            {
                var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousedashboardsummarydatabyowner/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
                if (response != null)
                {
                    DashBoardSummaryData = JsonConvert.DeserializeObject<PropertyHouseSummary>(response.Data.ToString());
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

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
