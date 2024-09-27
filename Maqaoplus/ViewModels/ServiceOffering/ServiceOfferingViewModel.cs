using System.ComponentModel;

namespace Maqaoplus.ViewModels.ServiceOffering
{
    public class ServiceOfferingViewModel
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public event PropertyChangedEventHandler PropertyChanged;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        public ServiceOfferingViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
    }
}
