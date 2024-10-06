using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Maqaoplus.Models
{
    public class PushNotificationReceived : ValueChangedMessage<string>
    {
        public PushNotificationReceived(string message) : base(message) { }
    }
}
