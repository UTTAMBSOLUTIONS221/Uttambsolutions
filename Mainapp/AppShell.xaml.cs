namespace Mainapp
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
