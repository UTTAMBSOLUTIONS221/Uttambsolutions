using Mainapp.Pages.Users;
using Mainapp.ViewModels;
namespace Mainapp
{
    public partial class AppShell : Shell
    {
        public AppShell(LoginPage loginPage)
        {
            InitializeComponent();
            this.BindingContext = new AppShellViewModel();
        }
    }
}
