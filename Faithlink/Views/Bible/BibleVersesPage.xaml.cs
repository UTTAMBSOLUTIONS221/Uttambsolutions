using Faithlink.ViewModels.Bibles;

namespace Faithlink.Views.Bible;

public partial class BibleVersesPage : ContentPage
{
    public BibleVersesPage(BibleViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}