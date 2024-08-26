using Firebase.Storage;
using Maqaoplus.ViewModels.PropertyHouse;
using System.Collections.ObjectModel;

namespace Maqaoplus.Views.PropertyHouse.Modal;

public partial class SystemPropertyHouseRoomImagesModalPage : ContentPage
{
    private const string FirebaseStorageBucket = "your-firebase-storage-bucket-url";
    private ObservableCollection<string> _imageUrls = new ObservableCollection<string>();

    public SystemPropertyHouseRoomImagesModalPage(PropertyHouseDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    private async void OnCaptureImageClicked(object sender, EventArgs e)
    {
        var result = await MediaPicker.CapturePhotoAsync();
        if (result != null)
        {
            await UploadAndSaveImageAsync(result);
        }
    }

    private async void OnPickImageClicked(object sender, EventArgs e)
    {
        var result = await MediaPicker.PickPhotoAsync();
        if (result != null)
        {
            await UploadAndSaveImageAsync(result);
        }
    }

    private async Task UploadAndSaveImageAsync(FileResult fileResult)
    {
        var stream = await fileResult.OpenReadAsync();
        var fileName = Path.GetFileName(fileResult.FullPath);

        // Upload to Firebase Storage
        var firebaseStorage = new FirebaseStorage(FirebaseStorageBucket);
        var imageUrl = await firebaseStorage
            .Child("images")
            .Child(fileName)
            .PutAsync(stream);

        // Save image URL to database (implementation needed)
        var url = await firebaseStorage
            .Child("images")
            .Child(fileName)
            .GetDownloadUrlAsync();

        _imageUrls.Add(url);
        // Save URL to database (implementation needed)
    }
}