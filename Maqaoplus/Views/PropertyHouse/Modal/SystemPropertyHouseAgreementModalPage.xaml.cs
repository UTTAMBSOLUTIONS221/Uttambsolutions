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
            // Ensure the stream is converted to a local copy and properly disposed
            using (var stream = await DrawBoard.GetImageStream(300, 300))
            {
                if (stream != null)
                {
                    var memoryStream = new MemoryStream();
                    await stream.CopyToAsync(memoryStream);
                    memoryStream.Seek(0, SeekOrigin.Begin); // Reset stream position

                    ImageView.Source = ImageSource.FromStream(() => memoryStream);

                    // Ensure stream is properly disposed to avoid recycling issues
                    stream.Dispose();
                }
            }
        });
    }

    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        DrawBoard.Lines.Clear();
    }
}