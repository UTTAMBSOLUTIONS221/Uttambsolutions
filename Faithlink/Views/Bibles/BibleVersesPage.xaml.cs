using Faithlink.ViewModels.Bibles;

namespace Faithlink.Views.Bibles;

public partial class BibleVersesPage : ContentPage
{
    public BibleVersesPage(BibleViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}