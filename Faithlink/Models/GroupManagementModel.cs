namespace Faithlink.Models
{
    public class GroupModel
    {
        public int Id { get; set; } // Unique identifier for the group
        public string Name { get; set; } // Name of the group
        public string Description { get; set; } // Brief description of the group
        public string OwnerUserId { get; set; } // User ID of the group creator/owner
        public List<string> MemberUserIds { get; set; } // List of user IDs who are members of the group
        public GroupPrivacy Privacy { get; set; } // Privacy setting of the group
        public GroupJoinSettings JoinSettings { get; set; } // Join settings for the group
        public GroupType Type { get; set; } // Type or category of the group
        public List<string> Tags { get; set; } // Tags associated with the group
        public string AvatarUrl { get; set; } // URL to the group's avatar or logo
        public List<GroupActivity> Activities { get; set; } // Recent activities or discussions in the group
        public GroupModeration Moderation { get; set; } // Moderation settings for the group
        public List<GroupDiscussion> Discussions { get; set; } // Discussion forums or channels in the group
        public List<GroupEvent> Events { get; set; } // Events or meetings scheduled for the group
        public List<GroupFile> Files { get; set; } // Files or documents shared within the group
        public GroupNotifications Notifications { get; set; } // Notification settings for the group

        // Constructor to initialize lists
        public GroupModel()
        {
            MemberUserIds = new List<string>();
            Tags = new List<string>();
            Activities = new List<GroupActivity>();
            Discussions = new List<GroupDiscussion>();
            Events = new List<GroupEvent>();
            Files = new List<GroupFile>();
        }
    }

    public enum GroupPrivacy
    {
        Public,
        Private,
        InviteOnly
    }

    public enum GroupJoinSettings
    {
        Open,
        ApprovalRequired,
        InvitationOnly
    }

    public enum GroupType
    {
        Social,
        Educational,
        Professional,
        Other
    }

    public class GroupActivity
    {
        public string UserId { get; set; } // User ID of the member performing the activity
        public DateTime Timestamp { get; set; } // Timestamp of the activity
        public string ActivityDetails { get; set; } // Details of the activity
    }

    public class GroupModeration
    {
        public List<string> AdminUserIds { get; set; } // User IDs of group admins
        public bool ContentModerationEnabled { get; set; } // Whether content moderation is enabled
    }

    public class GroupDiscussion
    {
        public string Title { get; set; } // Title of the discussion thread
        public string Description { get; set; } // Description or topic of the discussion
        public List<GroupMessage> Messages { get; set; } // Messages or posts within the discussion
    }

    public class GroupMessage
    {
        public string UserId { get; set; } // User ID of the message sender
        public DateTime Timestamp { get; set; } // Timestamp of the message
        public string Content { get; set; } // Content of the message
    }

    public class GroupEvent
    {
        public string Title { get; set; } // Title of the event
        public DateTime StartTime { get; set; } // Start time of the event
        public DateTime EndTime { get; set; } // End time of the event
        public string Location { get; set; } // Location or venue of the event
        public string Description { get; set; } // Description or details of the event
    }

    public class GroupFile
    {
        public string FileName { get; set; } // Name of the file
        public string FileType { get; set; } // Type or format of the file
        public string FileUrl { get; set; } // URL to access/download the file
    }

    public class GroupNotifications
    {
        public bool EmailNotificationsEnabled { get; set; } // Whether email notifications are enabled
        public bool PushNotificationsEnabled { get; set; } // Whether push notifications are enabled
    }
}