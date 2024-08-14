using Maqaoplus.ViewModels.PropertyHouse;
using System.Collections.Specialized;

namespace Maqaoplus.Views.PropertyHouse
{
    [QueryProperty(nameof(PropertyId), "PropertyId")]

    public partial class PropertyHousesDetailPage : ContentPage
    {
        public long PropertyId { get; set; }
        private PropertyHouseDetailViewModel _viewModel;


        public PropertyHousesDetailPage(Services.ServiceProvider serviceProvider)
        {
            InitializeComponent();
            _viewModel = new PropertyHouseDetailViewModel(serviceProvider);
            this.BindingContext = _viewModel;
            // Subscribe to the CollectionChanged event
            _viewModel.Rooms.CollectionChanged += OnItemsCollectionChanged;

            // Set default layout
            UpdateLayout();
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is PropertyHouseDetailViewModel viewModel)
            {
                viewModel.SetPropertyId(PropertyId);
            }
        }

        private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // Update layout when items change
            UpdateLayout();
        }

        private void UpdateLayout()
        {
            if (_viewModel.Rooms.Count > 0)
            {
                var itemCount = _viewModel.Rooms.Count;
                var span = CalculateSpan(itemCount);
                PropertyHousesCollectionView.ItemsLayout = new GridItemsLayout(span, ItemsLayoutOrientation.Vertical);
            }
        }

        private int CalculateSpan(int itemCount)
        {
            // Adjust span based on item count
            if (itemCount <= 1)
                return 1; // One items per row

            if (itemCount <= 2)
                return 2; // Two items per row

            if (itemCount <= 4)
                return 2; // Two items per row

            return 2; // Four items per row
        }
    }
}