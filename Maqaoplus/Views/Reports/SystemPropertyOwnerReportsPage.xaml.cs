using Maqaoplus.ViewModels.Reports;
namespace Maqaoplus.Views.Reports;

public partial class SystemPropertyOwnerReportsPage : ContentPage
{
    public SystemPropertyOwnerReportsPage(SystemReportsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}