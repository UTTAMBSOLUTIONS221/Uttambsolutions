namespace Maqaoplus.Views.Dashboards;

public partial class UserDashboardPage : ContentPage
{
    private PropertyHouseViewModel _viewModel;
    public UserDashboardPage(Services.ServiceProvider serviceProvider)
    {
        InitializeComponent();
        _viewModel = new PropertyHouseViewModel(serviceProvider);
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
}