using Firebase.Storage;
using Maqaoplus.ViewModels.PropertyHouseAgent;

namespace Maqaoplus.Views.PropertyHouseAgent.Modal;
public partial class SystemPropertyHouseAgentImagesModalPage : ContentPage
{
    private const string FirebaseStorageBucket = "uttambsolutions-4ec2a.appspot.com";

    public SystemPropertyHouseAgentImagesModalPage(PropertyHouseAgentViewModel viewModel)
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
        var uploadTask = firebaseStorage.Child("maqaoplus").Child("houseroom").Child(fileName).PutAsync(stream);
        var imagePath = await uploadTask;
        // Get the download URL
        //var url = await firebaseStorage.Child(imagePath).GetDownloadUrlAsync();
        await ((PropertyHouseAgentViewModel)BindingContext).SavePropertyHouseImageasync(imagePath);
    }
}