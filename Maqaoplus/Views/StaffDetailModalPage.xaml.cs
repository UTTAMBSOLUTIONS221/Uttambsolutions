using DBL.Entities;
using System.Windows.Input;

namespace Maqaoplus.Views;
public partial class StaffDetailModalPage : ContentPage
{
    public ICommand OkCommand { get; }
    public ICommand CancelCommand { get; }

    public StaffDetailModalPage(SystemStaff staffData, ICommand okCommand, ICommand cancelCommand)
    {
        InitializeComponent();
        BindingContext = staffData;
        OkCommand = okCommand;
        CancelCommand = cancelCommand;
    }
}