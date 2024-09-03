using Maqaoplus.ViewModels.HouseTenant;

namespace Maqaoplus.Views.Dashboards;
public partial class UserDashboardPage : ContentPage
{
    public UserDashboardPage(Propertyhousetenantviewmodel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is Propertyhousetenantviewmodel viewModel && viewModel.LoadItemsCommand.CanExecute(null))
        {
            viewModel.LoadItemsCommand.Execute(null);
        }
    }
    private async void OnWhatsAppButtonClicked(object sender, EventArgs e)
    {
        var phoneNumber = "0717850720";
        var currentTime = DateTime.Now.TimeOfDay;

        // Determine the appropriate greeting based on the time of day
        string greeting;
        if (currentTime < new TimeSpan(12, 0, 0)) // Before 12 PM
        {
            greeting = "Good Morning";
        }
        else if (currentTime < new TimeSpan(18, 0, 0)) // Before 6 PM
        {
            greeting = "Good Afternoon";
        }
        else // After 6 PM
        {
            greeting = "Good Evening";
        }

        // Construct the message
        string fullName = App.UserDetails.Usermodel.Fullname;
        var message = $"{greeting} {fullName}, welcome to Maqao plus from tenant department. Your number one property management system. We are here to help";
        var uri = new Uri($"https://wa.me/{phoneNumber}?text={Uri.EscapeDataString(message)}");
        var success = await Launcher.TryOpenAsync(uri);

        if (!success)
        {
            await DisplayAlert("Error", "Unable to open WhatsApp. Please ensure it is installed.", "OK");
        }
    }
}