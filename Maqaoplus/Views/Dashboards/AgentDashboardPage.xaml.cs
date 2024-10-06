using Maqaoplus.ViewModels.Dashboards;
#if ANDROID
using Android.Provider;
using Android.Content;
#endif

namespace Maqaoplus.Views.Dashboards;

public partial class AgentDashboardPage : ContentPage
{
    public AgentDashboardPage(SummaryDashBoardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
#if ANDROID
        // Fetch Android ID on Android
        string androidId = Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Settings.Secure.AndroidId);
#else
    // Provide a default or placeholder value for other platforms
    string androidId = "NotApplicable"; 
#endif

        if (BindingContext is SummaryDashBoardViewModel viewModel && viewModel.LoadAgentSummaryCommand.CanExecute(null))
        {
            await ((SummaryDashBoardViewModel)BindingContext).Savesystemuserdevicedata(androidId);
            viewModel.LoadAgentSummaryCommand.Execute(null);
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
        var message = $"{greeting} {fullName}, welcome to Maqao plus from agency module. Your number one property management system. We are here to help";

        var uri = new Uri($"https://wa.me/{phoneNumber}?text={Uri.EscapeDataString(message)}");
        var success = await Launcher.TryOpenAsync(uri);

        if (!success)
        {
            await DisplayAlert("Error", "Unable to open WhatsApp. Please ensure it is installed.", "OK");
        }
    }
}