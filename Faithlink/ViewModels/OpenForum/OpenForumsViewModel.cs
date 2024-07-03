using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Faithlink.Models;
using Faithlink.Services;
using System.Collections.ObjectModel;

namespace Faithlink.ViewModels
{
    public partial class OpenForumsViewModel : ObservableObject
    {
        private readonly IOpenForumsApiService _openForumsApiService;

        public ObservableCollection<OpenForum> OpenForums { get; } = new ObservableCollection<OpenForum>();

        public OpenForumsViewModel(IOpenForumsApiService openForumsApiService)
        {
            _openForumsApiService = openForumsApiService;
            LoadOpenForumsCommand = new AsyncRelayCommand(LoadOpenForumsAsync);
            JoinCommand = new AsyncRelayCommand<int>(JoinForumAsync);
        }

        public IAsyncRelayCommand LoadOpenForumsCommand { get; }
        public IAsyncRelayCommand<int> JoinCommand { get; }

        private async Task LoadOpenForumsAsync()
        {
            var forums = await _openForumsApiService.GetOpenForumsAsync();
            OpenForums.Clear();
            foreach (var forum in forums)
            {
                OpenForums.Add(forum);
            }
        }

        private async Task JoinForumAsync(int forumId)
        {
            await _openForumsApiService.JoinForumAsync(forumId);
            // Optionally reload forums or update UI to reflect joined forum
        }
    }
}