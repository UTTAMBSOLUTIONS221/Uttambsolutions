using Microsoft.Maui.Controls;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace Faithlink.ViewModels
{
    public class OpenForumsViewModel : BindableObject
    {
        private ObservableCollection<OpenForum> _openForums;
        public ObservableCollection<OpenForum> OpenForums
        {
            get => _openForums;
            set
            {
                _openForums = value;
                OnPropertyChanged();
            }
        }

        public OpenForumsViewModel()
        {
            // Initialize OpenForums collection (populate from database)
            LoadOpenForums();
        }

        private void LoadOpenForums()
        {
            // Simulated data for demonstration
            var forums = new ObservableCollection<OpenForum>
            {
                new OpenForum { ForumId = 1, ForumName = "Forum A", OpenTime = "09:00 AM" },
                new OpenForum { ForumId = 2, ForumName = "Forum B", OpenTime = "10:30 AM" },
                new OpenForum { ForumId = 3, ForumName = "Forum C", OpenTime = "12:00 PM" }
                // Fetch actual data from database here
            };

            // Determine if each forum is open based on current time
            foreach (var forum in forums)
            {
                if (IsForumOpen(forum.OpenTime))
                {
                    forum.IsOpen = true;
                }
                else
                {
                    forum.IsOpen = false;
                }
            }

            OpenForums = forums;
        }

        private bool IsForumOpen(string openTime)
        {
            // Compare current time with the openTime of the forum
            var currentTime = DateTime.Now.TimeOfDay;
            var forumOpenTime = TimeSpan.Parse(openTime);

            // Forum is considered open if current time is after or equal to openTime
            return currentTime >= forumOpenTime;
        }
    }

    public class OpenForum
    {
        public int ForumId { get; set; }
        public string ForumName { get; set; }
        public string OpenTime { get; set; }
        public bool IsOpen { get; set; }
        // Add more properties as needed
    }
}
