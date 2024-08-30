using Maqaoplus.ViewModels.PropertyHouseAgent;

namespace Maqaoplus.Views.PropertyHouseAgent.Modal;
public partial class AddSystemPropertyHouseAgentModalPage : ContentPage
{
    public AddSystemPropertyHouseAgentModalPage(PropertyHouseAgentViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}