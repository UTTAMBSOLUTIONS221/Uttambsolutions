namespace Maqaoplus.Views.PropertyHouse
{
    [QueryProperty(nameof(PropertyId), "PropertyId")]
    public partial class PropertyHousesDetailPage : ContentPage
    {
        public long PropertyId { get; set; }

        public PropertyHousesDetailPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Use PropertyId to load details or set up the page
            System.Diagnostics.Debug.WriteLine($"Property ID received: {PropertyId}");
        }
    }
}
