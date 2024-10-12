using Parceldrop.ViewModels;

namespace Parceldrop
{
    public partial class MainPage : ContentPage
    {
        private MainPageViewModel _viewModel;
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }
        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();
        //    if (_viewModel.LoadMaqaoplussummaryCommand.CanExecute(null))
        //    {
        //        _viewModel.LoadMaqaoplussummaryCommand.Execute(null);
        //    }
        //}
    }
}
