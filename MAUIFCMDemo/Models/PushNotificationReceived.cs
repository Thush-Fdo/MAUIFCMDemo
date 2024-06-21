using CommunityToolkit.Mvvm.Messaging.Messages;
 
namespace MAUIFCMDemo.Models
{
    public class PushNotificationReceived : ValueChangedMessage<string>
    {
        public PushNotificationReceived(string message): base(message) { }
    }
}