using DBL.Models;

namespace Esacco
{
    public partial class App : Application
    {
        public static UsermodelResponce UserDetails;
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
    }
}
