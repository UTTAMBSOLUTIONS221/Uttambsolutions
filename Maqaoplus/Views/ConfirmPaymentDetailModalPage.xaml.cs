using DBL.Entities;
using System.Windows.Input;

namespace Maqaoplus.Views;

public partial class ConfirmPaymentDetailModalPage : ContentPage
{
    public partial class ConfirmPaymentDetailModalPage : ContentPage
    {
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public ConfirmPaymentDetailModalPage(SystemStaff staffData, ICommand okCommand, ICommand cancelCommand)
        {
            InitializeComponent();
            BindingContext = staffData;
            OkCommand = okCommand;
            CancelCommand = cancelCommand;

            // Bind the commands to the buttons explicitly
            okButton.Command = OkCommand;
            cancelButton.Command = CancelCommand;
        }
    }
}