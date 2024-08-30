using Maqaoplus.ViewModels.PropertyHouseAgent;

namespace Maqaoplus.Views.PropertyHouseAgent.Modal;

public partial class SystemPropertyAgentHouseRoomCheckListsModalPage : ContentPage
{
    public SystemPropertyAgentHouseRoomCheckListsModalPage(PropertyHouseAgentDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}