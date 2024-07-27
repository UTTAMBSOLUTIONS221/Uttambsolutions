using Mainapp.ViewModels;

namespace Mainapp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            this.BindingContext = new AppShellViewModel();


        }
    }
}
