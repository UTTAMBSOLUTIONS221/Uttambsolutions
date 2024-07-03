using Faithlink.ViewModels;
using Faithlink.ViewModels.OpenForum;

namespace Faithlink.Views.OpenForums
{
    public partial class OpenForumsPage : ContentPage
    {
        public OpenForumsPage()
        {
            InitializeComponent();
            BindingContext = new OpenForumsViewModel();
        }
    }
}
