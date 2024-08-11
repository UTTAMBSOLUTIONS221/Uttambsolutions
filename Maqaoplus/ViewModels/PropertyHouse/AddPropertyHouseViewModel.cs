using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Maqaoplus.ViewModels.PropertyHouse
{
    public class AddPropertyHouseViewModel : INotifyPropertyChanged
    {
        private string _firstName;
        private string _lastName;
        private string _emailAddress;
        private string _phoneNumber;
        private string _password;
        private string _confirmPassword;
        private bool _isProcessing;

        public event PropertyChangedEventHandler PropertyChanged;
        private readonly Services.ServiceProvider _serviceProvider;

        public AddPropertyHouseViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
