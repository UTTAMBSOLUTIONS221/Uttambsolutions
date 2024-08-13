using Maqaoplus.ViewModels.PropertyHouse;
using System.Collections.Specialized;
namespace Maqaoplus.Views.PropertyHouse;

public partial class PropertyHousesRoomTenantsPage : ContentPage
{
    private PropertyHousesRoomTenantsViewModel _viewModel;

    public PropertyHousesRoomTenantsPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHousesRoomTenantsViewModel(serviceProvider);
        this.BindingContext = _viewModel;
        // Subscribe to the CollectionChanged event
        _viewModel.Items.CollectionChanged += OnItemsCollectionChanged;

        // Set default layout
        PropertyHousesCollectionView.ItemsLayout = new GridItemsLayout(2, ItemsLayoutOrientation.Vertical);

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadItemsCommand.CanExecute(null))
        {
            _viewModel.LoadItemsCommand.Execute(null);
        }
    }
    private void OnItemsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        if (PropertyHousesCollectionView.ItemsSource != null)
        {
            var itemCount = _viewModel.Items.Count;
            var span = CalculateSpan(itemCount);
            PropertyHousesCollectionView.ItemsLayout = new GridItemsLayout(span, ItemsLayoutOrientation.Vertical);
        }
    }
    private int CalculateSpan(int itemCount)
    {
        if (itemCount == 1)
            return 2; // Fill the page row

        if (itemCount == 2 || itemCount == 4)
            return 2; // Divide by two

        if (itemCount == 3 || itemCount == 5)
            return 3; // Three on top and two below or similar

        return 2; // Default span
    }
}