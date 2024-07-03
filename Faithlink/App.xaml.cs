using Faithlink.Components.Views;
using Faithlink.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Faithlink
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new LoginPage(new AuthenticationService()));
        }
    }
}
