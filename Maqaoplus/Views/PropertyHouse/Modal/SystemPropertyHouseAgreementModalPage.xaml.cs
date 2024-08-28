using Firebase.Storage;
using Maqaoplus.ViewModels.PropertyHouse;
using SkiaSharp;

namespace Maqaoplus.Views.PropertyHouse.Modal;

public partial class SystemPropertyHouseAgreementModalPage : ContentPage
{
    public SystemPropertyHouseAgreementModalPage(PropertyHouseViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void DrawBoard_DrawingLineCompleted(System.Object sender, CommunityToolkit.Maui.Core.DrawingLineCompletedEventArgs e)
    {
        await ImageView.Dispatcher.DispatchAsync(async () =>
        {
            var targetWidth = 300;
            var targetHeight = 100;

            // Get the image stream from DrawBoard
            using (var originalStream = await DrawBoard.GetImageStream(targetWidth, targetHeight))
            {
                if (originalStream != null)
                {
                    // Use a MemoryStream to buffer the image data
                    using (var memoryStream = new MemoryStream())
                    {
                        await originalStream.CopyToAsync(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin); // Reset stream position

                        // Resize the image and create a new MemoryStream for it
                        using (var resizedStream = await ResizeImageAsync(memoryStream, targetWidth, targetHeight))
                        {
                            ImageView.Source = ImageSource.FromStream(() => new MemoryStream(resizedStream.ToArray()));
                        }
                    }
                }
            }
        });
    }

    private async Task<MemoryStream> ResizeImageAsync(MemoryStream imageStream, int width, int height)
    {
        // Load the image into a SkiaSharp bitmap
        using (var originalBitmap = SKBitmap.Decode(imageStream))
        {
            // Create a new bitmap with the desired size
            var resizedBitmap = new SKBitmap(width, height);

            // Create a new canvas to draw on the bitmap
            using (var canvas = new SKCanvas(resizedBitmap))
            {
                // Fill the background with white
                canvas.Clear(SKColors.White);

                // Calculate the scaling ratio
                var ratioX = (float)width / originalBitmap.Width;
                var ratioY = (float)height / originalBitmap.Height;
                var ratio = Math.Min(ratioX, ratioY);

                // Calculate the position (center the image)
                var newWidth = originalBitmap.Width * ratio;
                var newHeight = originalBitmap.Height * ratio;
                var x = (width - newWidth) / 2;
                var y = (height - newHeight) / 2;

                // Scale and draw the original bitmap onto the new bitmap
                var destRect = new SKRect(x, y, x + newWidth, y + newHeight);
                canvas.DrawBitmap(originalBitmap, destRect);
            }

            // Save the resized bitmap to a memory stream
            var resizedStream = new MemoryStream();
            using (var image = SKImage.FromBitmap(resizedBitmap))
            using (var data = image.Encode())
            {
                data.SaveTo(resizedStream);
                resizedStream.Seek(0, SeekOrigin.Begin);
            }

            return resizedStream;
        }
    }

    private async void Button_Save_Signature_Clicked(object sender, EventArgs e)
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
        var uploadTask = firebaseStorage.Child("maqaoplus").Child(fileName).PutAsync(stream);
        var downloadUrl = await uploadTask;
        return downloadUrl;
    }
}