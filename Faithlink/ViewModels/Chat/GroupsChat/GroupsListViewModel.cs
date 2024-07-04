using Faithlink.Models;
using Faithlink.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Faithlink.ViewModels.Chat.GroupsChat
{
    public class GroupsListViewModel : BaseViewModel
    {
        private readonly IGroupManagementService _groupService;
        private readonly long _userId; // Assuming you have access to the session user's UserId

        private ObservableCollection<GroupModel> _groups;
        public ObservableCollection<GroupModel> Groups
        {
            get => _groups;
            set => SetProperty(ref _groups, value);
        }

        public ICommand JoinGroupCommand { get; }

        public GroupsListViewModel(long userId)
        {
            _groupService = new GroupManagementService(); // Initialize with appropriate service
            _userId = userId;

            JoinGroupCommand = new Command<GroupModel>(async (group) => await OnJoinGroupAsync(group));

            // Load groups asynchronously when ViewModel is initialized
            Task.Run(async () => await LoadGroupsAsync());
        }

        public async Task LoadGroupsAsync()
        {
            IsBusy = true;

            // Example: Load groups from service asynchronously
            var groups = await _groupService.LoadGroupsAsync();

            Groups = new ObservableCollection<GroupModel>(groups);

            IsBusy = false; // Reset IsBusy or IsLoading property
        }

        public async Task OnJoinGroupAsync(GroupModel group)
        {
            IsBusy = true; // Set IsBusy or IsLoading property to indicate joining process

            // Example: Call service method to join the group with current user's UserId
            //var result = await _groupService.JoinGroupAsync(group, _userId);


            IsBusy = false; // Reset IsBusy or IsLoading property
        }
    }
}
