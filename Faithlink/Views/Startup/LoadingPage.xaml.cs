using Faithlink.ViewModels.Startup;

namespace Faithlink.Views.Startup;

public partial class LoadingPage : ContentPage
{
    public LoadingPage(LoadingPageViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}