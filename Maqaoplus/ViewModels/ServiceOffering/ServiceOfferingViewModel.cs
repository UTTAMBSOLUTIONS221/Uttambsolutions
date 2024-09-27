using DBL.Entities;
using DBL.Models;
using Maqaoplus.Views.ServiceOffering.Modals;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.ServiceOffering
{
    public class ServiceOfferingViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public event PropertyChangedEventHandler PropertyChanged;
        public ICommand AddServiceOfferingCommand { get; }
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";


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

        private ServiceOfferings _serviceofferingsData;
        public ServiceOfferings ServiceofferingsData
        {
            get => _serviceofferingsData;
            set
            {
                if (_serviceofferingsData != value)
                {
                    _serviceofferingsData = value;
                    OnPropertyChanged(nameof(ServiceofferingsData));
                }
            }
        }

        private ObservableCollection<ListModel> _servicetype;
        public ObservableCollection<ListModel> Servicetype
        {
            get => _servicetype;
            set
            {
                _servicetype = value;
                OnPropertyChanged();
            }
        }
        private ListModel _selectedServicetype;
        public ListModel SelectedServicetype
        {
            get => _selectedServicetype;
            set
            {
                _selectedServicetype = value;
                OnPropertyChanged();
            }
        }

        public ServiceOfferingViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            AddServiceOfferingCommand = new Command<ServiceOfferings>(async (service) => { var serviceId = service?.Serviceid ?? 0; await AddServiceOfferingAsync(serviceId); });
        }

        private async Task AddServiceOfferingAsync(long Serviceid)
        {
            IsProcessing = true;
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhousedetaildatabyid/" + Serviceid, HttpMethod.Get, null);
            if (response != null)
            {
                ServiceofferingsData = JsonConvert.DeserializeObject<ServiceOfferings>(response.Data.ToString());
                if (Serviceid > 0)
                {
                    if (ServiceofferingsData != null)
                    {
                        if (ServiceofferingsData.Servicetypeid > 0)
                        {
                            SelectedServicetype = Servicetype.FirstOrDefault(x => x.Value == _serviceofferingsData.Servicetypeid.ToString());
                        }
                    }
                }
            }
            var modalPage = new AddServiceOfferingModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
