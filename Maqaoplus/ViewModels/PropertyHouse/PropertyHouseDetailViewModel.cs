using DBL.Entities;
using Newtonsoft.Json;

namespace Maqaoplus.ViewModels.PropertyHouse
{
    public class PropertyHouseDetailViewModel : BaseViewModel
    {
        private Systemproperty _property;
        public Systemproperty Property
        {
            get => _property;
            set => SetProperty(ref _property, value);
        }

        public void LoadProperty(string jsonProperty)
        {
            Property = JsonConvert.DeserializeObject<Systemproperty>(jsonProperty);
        }
    }
}