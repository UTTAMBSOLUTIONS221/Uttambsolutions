
using Maqaoplus.ViewModels.Startup;
namespace Maqaoplus.Views.Startup;

public partial class LoadingPage : ContentPage
{
    public LoadingPage(LoadingPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}