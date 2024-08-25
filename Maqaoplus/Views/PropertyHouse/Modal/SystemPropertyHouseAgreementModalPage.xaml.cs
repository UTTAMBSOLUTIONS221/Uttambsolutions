using Firebase.Storage;
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



    private async Task Button_Save_Signature_Clicked()
    {
        var filePath = Path.Combine(FileSystem.AppDataDirectory, "signature.png");

        // Save the drawn signature to a file
        using (var stream = await DrawBoard.GetImageStream(300, 100))
        using (var fileStream = File.Create(filePath))
        {
            await stream.CopyToAsync(fileStream);
        }

        // Upload the image to Firebase Storage and get the URL
        var imageUrl = await UploadImageToFirebaseAsync(filePath, "signature.png");

        // Use the image URL with the ViewModel or make an API call
        await ((PropertyHouseViewModel)BindingContext).AgreeToPropertyHouseAgreementasync(imageUrl);
    }
    void Button_Clicked(System.Object sender, System.EventArgs e)
    {
        DrawBoard.Lines.Clear();
    }
    public async Task<string> UploadImageToFirebaseAsync(string filePath, string fileName)
    {
        var stream = File.Open(filePath, FileMode.Open);
        var firebaseStorage = new FirebaseStorage("uttambsolutions-4ec2a.appspot.com");
        var uploadTask = firebaseStorage.Child("images").Child(fileName).PutAsync(stream);
        var downloadUrl = await uploadTask;
        return downloadUrl;
    }
}