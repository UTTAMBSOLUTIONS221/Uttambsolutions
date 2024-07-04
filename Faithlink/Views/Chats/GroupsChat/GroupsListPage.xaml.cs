using Faithlink.ViewModels.Chat.GroupsChat;

namespace Faithlink.Views.Chats.GroupsChat;

public partial class GroupsListPage : ContentPage
{
    public GroupsListPage(GroupsListViewModel viewModel)
    {
        InitializeComponent();
        this.BindingContext = viewModel;
    }
}