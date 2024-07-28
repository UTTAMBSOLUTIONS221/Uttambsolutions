using DBL.Models;
namespace Mainapp
{
    public partial class App : Application
    {
        public static UsermodeldataResponce UserDetails;
        public App(AppShell appShell)
        {
            InitializeComponent();

            MainPage = appShell;
        }
    }
}
