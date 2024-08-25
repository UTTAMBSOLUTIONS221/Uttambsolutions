using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse.Modal;

public partial class SystemPropertyHouseAgreementModalPage : ContentPage
{
    public SystemPropertyHouseAgreementModalPage(PropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    private void DrawBoard_DrawingLineCompleted(System.Object sender, CommunityToolkit.Maui.Core.DrawingLineCompletedEventArgs e)
    {
        ImageView.Dispatcher.Dispatch(async () =>
        {
            var stream = await DrawBoard.GetImageStream(300, 300);
            ImageView.Source = ImageSource.FromStream(() => stream);
        });
    }
    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        DrawBoard.Lines.Clear();
    }
}