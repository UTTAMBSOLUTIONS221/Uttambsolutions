using DBL.Models;
namespace Maqaoplus
{
    public partial class App : Application
    {
        public static UsermodelResponce UserDetails;
        public App(AppShell appShell)
        {
            InitializeComponent();

            MainPage = appShell;
            CheckConnectivity();
        }
        private void CheckConnectivity()
        {
            if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                // Handle no internet connection
                MainPage.DisplayAlert("No Internet", "You are not connected to the internet.", "OK");
            }
        }
    }
}
