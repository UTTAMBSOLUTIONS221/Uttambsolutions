using Maqaoplus.ViewModels.Dashboards;

namespace Maqaoplus.Views.Dashboards;

public partial class PropertyOwnerDashboardPage : ContentPage
{
    public PropertyOwnerDashboardPage(SummaryDashBoardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SummaryDashBoardViewModel viewModel && viewModel.LoadOwnerSummaryCommand.CanExecute(null))
        {
            viewModel.LoadOwnerSummaryCommand.Execute(null);
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
        string fullName = App.UserDetails.Usermodel.Fullname;

        // Construct the message
        var message = $"{greeting} {fullName}, welcome to Maqao plus from property owner department. Your number one property management system. We are here to help";

        var success = await OpenWhatsAppChatAsync(phoneNumber, message);

        if (!success)
        {
            await DisplayAlert("Error", "Unable to open WhatsApp. Please ensure it is installed.", "OK");
        }
    }


    private async Task<bool> OpenWhatsAppChatAsync(string phoneNumber, string message)
    {
        try
        {
            // Construct the URL
            var url = $"whatsapp://send?phone={phoneNumber}&text={Uri.EscapeDataString(message)}";

            // Check if the device can handle the URL scheme
            if (await CanOpenUrlAsync(url))
            {
                // Open the URL
                await Launcher.OpenAsync(new Uri(url));
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error opening WhatsApp: {ex.Message}");
            return false;
        }
    }

    private async Task<bool> CanOpenUrlAsync(string url)
    {
        try
        {
            var uri = new Uri(url);
            return await Launcher.CanOpenAsync(uri);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking URL: {ex.Message}");
            return false;
        }
    }
}