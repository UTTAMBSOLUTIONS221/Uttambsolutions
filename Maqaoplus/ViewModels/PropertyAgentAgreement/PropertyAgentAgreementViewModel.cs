using DBL.Models;
using Maqaoplus.Views.PropertyHouseAgentAgreement.Modal;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Maqaoplus.ViewModels.PropertyHouseAgentAgreement
{
    public class PropertyAgentAgreementViewModel : INotifyPropertyChanged
    {
        private readonly Services.ServiceProvider _serviceProvider;
        public event PropertyChangedEventHandler PropertyChanged;
        public string CopyrightText => $"© 2020 - {DateTime.Now.Year}  UTTAMB SOLUTIONS LIMITED";
        private OwnerTenantAgreementDetailData _ownerTenantAgreementDetailData;
        public ICommand ViewPropertyAgentAgreementCommand { get; }

        private bool _isProcessing;
        public bool IsProcessing
        {
            get => _isProcessing;
            set
            {
                _isProcessing = value;
                OnPropertyChanged(nameof(IsProcessing));
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


        // Parameterless constructor for XAML support
        public PropertyAgentAgreementViewModel(Services.ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            ViewPropertyAgentAgreementCommand = new Command(async () => await ViewPropertyAgentAgreementDetails());
        }
        private async Task ViewPropertyAgentAgreementDetails()
        {
            IsProcessing = true;
            var response = await _serviceProvider.CallAuthWebApi<object>("/api/PropertyHouse/Getsystempropertyhouseagreementdetaildatabyagentid/" + App.UserDetails.Usermodel.Userid, HttpMethod.Get, null);
            if (response != null)
            {
                OwnerTenantAgreementDetailData = JsonConvert.DeserializeObject<OwnerTenantAgreementDetailData>(response.Data.ToString());
            }
            var modalPage = new SystemPropertyHouseAgentAgreementModalPage(this);
            await Application.Current.MainPage.Navigation.PushModalAsync(modalPage);
            IsProcessing = false;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
