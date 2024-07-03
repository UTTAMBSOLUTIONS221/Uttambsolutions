using Faithlink.ViewModels;

namespace Faithlink
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
