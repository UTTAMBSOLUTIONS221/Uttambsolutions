using Parceldrop.ViewModels.Parceldrop;

namespace Parceldrop.Views.Parceldrop;

public partial class CollectionDropCentersPage : ContentPage
{
    private ParcelDropViewModel _viewModel;
    public CollectionDropCentersPage(ParcelDropViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_viewModel.LoadCurrentParcelDropCommand.CanExecute(null))
        {
            _viewModel.LoadCurrentParcelDropCommand.Execute(null);
        }
    }
}
