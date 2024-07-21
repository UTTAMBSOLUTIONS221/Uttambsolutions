using Mainapp.Miniapps.Controls;
using Mainapp.Miniapps.ViewModels;
namespace Mainapp.Miniapps.Apps.Church.ViewModels
{
    public partial class ChurchDashBoardViewModel : BaseViewModel
    {
        public ChurchDashBoardViewModel()
        {
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
        }
    }
}
