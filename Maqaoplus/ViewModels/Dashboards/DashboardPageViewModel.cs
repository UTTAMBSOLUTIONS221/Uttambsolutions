using Maqaoplus.Controls;
using System.ComponentModel;
namespace Maqaoplus.ViewModels.Dashboards
{
    public partial class DashboardPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public DashboardPageViewModel()
        {
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
        }
    }
}
