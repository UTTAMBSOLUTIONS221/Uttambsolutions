using App.Pages;
using App.Pages.Users;

namespace App
{
    public partial class AppShell : Shell
    {
        public AppShell(LoginPage loginPage)
        {
            InitializeComponent();
            Routing.RegisterRoute("DashBoardPage", typeof(DashBoardPage));
            this.CurrentItem = loginPage;
        }

    }
}
