using Mainapp.Miniapps.Apps.Church;
using Mainapp.Pages.Users;
namespace Mainapp
{
    public partial class AppShell : Shell
    {
        public AppShell(LoginPage loginPage)
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ChurchDashBoardPage), typeof(ChurchDashBoardPage));
            this.CurrentItem = loginPage;
        }
    }
}
