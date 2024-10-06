using Maqaoplus.ViewModels.Startup;
namespace Maqaoplus.Views.Startup;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
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
        var message = $"{greeting}, welcome to Maqao plus. Your number one property management system. We are here to help";
        var uri = new Uri($"https://wa.me/{phoneNumber}?text={Uri.EscapeDataString(message)}");
        var success = await Launcher.TryOpenAsync(uri);

        if (!success)
        {
            await DisplayAlert("Error", "Unable to open WhatsApp. Please ensure it is installed.", "OK");
        }
    }
}