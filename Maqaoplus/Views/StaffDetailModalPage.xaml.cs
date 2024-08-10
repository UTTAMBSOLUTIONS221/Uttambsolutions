using DBL.Entities;
using System.Windows.Input;

namespace Maqaoplus.Views;
public partial class StaffDetailModalPage : ContentPage
{
    public ICommand OkCommand { get; }
    public ICommand CancelCommand { get; }

    public StaffDetailModalPage(SystemStaff tenantStaffData, ICommand okCommand, ICommand cancelCommand)
    {
        InitializeComponent();
        BindingContext = tenantStaffData;
        OkCommand = okCommand;
        CancelCommand = cancelCommand;
    }
}