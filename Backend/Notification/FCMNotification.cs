using FirebaseAdmin.Messaging;

namespace Backend.Notification;

public class FCMNotification
{
    public async Task<string> SendNotificationAsync(string deviceToken, string title, string body)
    {
        var message = new Message()
        {
            Notification = new FirebaseAdmin.Messaging.Notification
            {
                Title = title,
                Body = body
            },
            Token = deviceToken
        };

        var response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
        return response; // This is the message ID
    }
}