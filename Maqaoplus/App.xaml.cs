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
        }
    }
}
