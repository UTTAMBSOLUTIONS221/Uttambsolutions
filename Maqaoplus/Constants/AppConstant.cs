using Maqaoplus.Controls;
using Maqaoplus.Views.Dashboards;

namespace Maqaoplus.Constants
{
    public class AppConstant
    {
        public async static Task AddFlyoutMenusDetails()
        {
            AppShell.Current.FlyoutHeader = new FlyoutHeaderControl();
            var adminDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(AdminDashboardPage)).FirstOrDefault();
            if (adminDashboardInfo != null) AppShell.Current.Items.Remove(adminDashboardInfo);

            var propertyOwnerDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(PropertyOwnerDashboardPage)).FirstOrDefault();
            if (propertyOwnerDashboardInfo != null) AppShell.Current.Items.Remove(propertyOwnerDashboardInfo);

            var propertyCaretakerDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(PropertyCaretakerDashboardPage)).FirstOrDefault();
            if (propertyCaretakerDashboardInfo != null) AppShell.Current.Items.Remove(propertyCaretakerDashboardInfo);


            var agentDashboardPageInfo = AppShell.Current.Items.Where(f => f.Route == nameof(AgentDashboardPage)).FirstOrDefault();
            if (agentDashboardPageInfo != null) AppShell.Current.Items.Remove(agentDashboardPageInfo);

            var userDashboardInfo = AppShell.Current.Items.Where(f => f.Route == nameof(UserDashboardPage)).FirstOrDefault();
            if (userDashboardInfo != null) AppShell.Current.Items.Remove(userDashboardInfo);


        }
    }
}
