using Maqaoplus.Controls;
namespace Maqaoplus.ViewModels.Dashboards
{
    public partial class DashboardPageViewModel : BaseViewModel
    {
        public DashboardPageViewModel()
        {
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
        }
    }
}
