using Firebase.Storage;
using Maqaoplus.ViewModels.PropertyHouse;

namespace Maqaoplus.Views.PropertyHouse.Modal
{
    public partial class SystemPropertyHouseRoomImagesModalPage : ContentPage
    {
        private const string FirebaseStorageBucket = "uttambsolutions-4ec2a.appspot.com";

        public SystemPropertyHouseRoomImagesModalPage(PropertyHouseDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = (PropertyHouseDetailViewModel)BindingContext;
            await LoadImageUrlsAsync(viewModel);
        }

        private async Task LoadImageUrlsAsync(PropertyHouseDetailViewModel viewModel)
        {
            if (viewModel != null)
            {
                // Assuming the view model has a property that provides image URLs
                //ImagesCollectionView.ItemsSource = viewModel.ImageUrls;
            }
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
            var uploadTask = firebaseStorage.Child("maqaoplus").Child(fileName).PutAsync(stream);
            var imagePath = await uploadTask;
            // Get the download URL
            var url = await firebaseStorage.Child(imagePath).GetDownloadUrlAsync();

            // Assuming the view model has a method to add image URLs
            var viewModel = (PropertyHouseDetailViewModel)BindingContext;
            //viewModel.ImageUrls.Add(url);

            // Optionally: Save URL to database (implementation needed)
        }
    }
}
