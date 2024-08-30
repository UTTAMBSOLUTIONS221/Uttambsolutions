using Maqaoplus.ViewModels.PropertyHouseAgent;

namespace Maqaoplus.Views.PropertyHouseAgent.Modal;

public partial class AgentHousesRoomDetailModalPage : ContentPage
{
    public AgentHousesRoomDetailModalPage(PropertyHouseAgentDetailViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}