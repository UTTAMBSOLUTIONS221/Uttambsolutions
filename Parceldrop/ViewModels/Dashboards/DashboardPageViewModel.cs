using Parceldrop.Controls;
using System.ComponentModel;

namespace Parceldrop.ViewModels.Dashboards
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
