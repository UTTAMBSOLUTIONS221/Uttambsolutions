using DBL.Entities;
using System.Windows.Input;

namespace Maqaoplus.Views;
public partial class StaffDetailModalPage : ContentPage
{
    public ICommand OkCommand { get; }
    public ICommand CancelCommand { get; }

    public StaffDetailModalPage(SystemStaff customerData, ICommand okCommand, ICommand cancelCommand)
    {
        InitializeComponent();
        BindingContext = customerData;
        OkCommand = okCommand;
        CancelCommand = cancelCommand;
    }
}