using FirebaseAdmin.Messaging;

namespace Backend.Notification;

public class FCMNotification
{
    private readonly ILogger<FCMNotification> _logger;

    public FCMNotification(ILogger<FCMNotification> logger)
    {
        _logger = logger;
    }

    public async Task<bool> SendNotificationAsync(string deviceToken, string title, string body)
    {
        try
        {
            var message = new Message()
            {
                Token = deviceToken,
                Notification = new FirebaseAdmin.Messaging.Notification()
                {
                    Title = title,
                    Body = body
                },
                Android = new AndroidConfig
                {
                    Priority = Priority.High
                },
                Apns = new ApnsConfig
                {
                    Headers = new Dictionary<string, string>
                    {
                        { "apns-priority", "10" }
                    }
                }
            };

            var result = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            _logger.LogInformation("Successfully sent message: {MessageId}", result);
            return true;
        }
        catch (FirebaseMessagingException ex)
        {
            _logger.LogError(ex, "FCM error for token: {Token}", deviceToken);
            throw; // Re-throw to let caller handle
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error sending notification");
            return false;
        }
    }
}