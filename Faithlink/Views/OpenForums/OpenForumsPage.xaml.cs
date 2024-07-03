using Faithlink.ViewModels;
namespace Faithlink.Views.OpenForums
{
    public partial class OpenForumsPage : ContentPage
    {
        public OpenForumsPage(OpenForumsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = (OpenForumsViewModel)BindingContext;
            if (viewModel.LoadOpenForumsCommand.CanExecute(null))
            {
                viewModel.LoadOpenForumsCommand.Execute(null);
            }
        }
    }
}
