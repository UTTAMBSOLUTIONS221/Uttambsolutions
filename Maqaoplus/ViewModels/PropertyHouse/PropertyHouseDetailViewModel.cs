using DBL.Entities;

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

        public PropertyHouseDetailViewModel()
        {
        }

        public PropertyHouseDetailViewModel(Systemproperty property)
        {
            Property = property;
        }
    }
}