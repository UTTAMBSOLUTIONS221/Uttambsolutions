using Firebase.Storage;
using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse.Modal
{
    public partial class SystemPropertyHouseRoomImagesModalPage : ContentPage
    {
        private const string FirebaseStorageBucket = "uttambsolutions-4ec2a.appspot.com";

        public SystemPropertyHouseRoomImagesModalPage(PropertyHouseViewModel viewModel)
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
            await ((PropertyHouseDetailViewModel)BindingContext).SavePropertyHouseRoomImageasync(imagePath);
        }
    }
}
