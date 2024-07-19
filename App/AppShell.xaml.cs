using App.Pages.Users;

namespace App
{
    public partial class AppShell : Shell
    {
        public AppShell(LoginPage loginPage)
        {
            InitializeComponent();
            this.CurrentItem = loginPage;
        }

    }
}
