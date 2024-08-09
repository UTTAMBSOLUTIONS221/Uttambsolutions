using Maqaoplus.ViewModels.Startup;
namespace Maqaoplus.Views.Startup;


[QueryProperty(nameof(UserId), "UserId")]
public partial class ValidateStaffAccountPage : ContentPage
{
    public long UserId { get; set; }
    private ValidateStaffAccountPageViewModel _viewModel;

    public ValidateStaffAccountPage(ValidateStaffAccountPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ValidateStaffAccountPageViewModel viewModel)
        {
            viewModel.SetUserId(UserId);
        }
    }
}