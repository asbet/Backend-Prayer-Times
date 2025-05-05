using Backend.Notification;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/notifications")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly FCMNotification _fcmNotification;

    public NotificationController(FCMNotification fcmNotification)
    {
        _fcmNotification = fcmNotification;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
    {
        try
        {
            var result = await _fcmNotification.SendNotificationAsync(request.DeviceToken, request.Title, request.Body);
            return Ok(new { MessageId = result });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}

public class NotificationRequest
{
    public string Title { get; set; }
    public string Body { get; set; }
    public string DeviceToken { get; set; }
}