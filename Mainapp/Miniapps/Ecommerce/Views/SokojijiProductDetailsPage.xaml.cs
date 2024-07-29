using DBL.Models;
using Mainapp.Miniapps.Ecommerce.ViewModels;
namespace Mainapp.Miniapps.Ecommerce.Views;

public partial class SokojijiProductDetailsPage : ContentPage
{
    public SokojijiProductDetailsPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is SokojijiProductDetailsViewModel viewModel)
        {
            if (Shell.Current.CurrentState.Parameters.TryGetValue("Product", out var product))
            {
                viewModel.Product = product as Organizationshopproductsdata;
            }
        }
    }
}