using System.Windows.Input;

namespace Maqaoplus.Views;

public partial class SystemReportDetailModalPage : ContentPage
{
    public ICommand OkCommand { get; }
    public ICommand CancelCommand { get; }

    public SystemReportDetailModalPage(ICommand okCommand, ICommand cancelCommand)
    {
        InitializeComponent();
        OkCommand = okCommand;
        CancelCommand = cancelCommand;

        // Bind the commands to the buttons explicitly
        okButton.Command = OkCommand;
        cancelButton.Command = CancelCommand;
    }
}