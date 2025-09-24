using System.Globalization;
using Backend.Notification;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;

namespace Backend;

public class PrayerNotificationService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PrayerNotificationService> _logger;

    public PrayerNotificationService(IServiceProvider serviceProvider, ILogger<PrayerNotificationService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    var db = scope.ServiceProvider.GetRequiredService<PrayerTimesDbContext>();
                    var fcm = scope.ServiceProvider.GetRequiredService<FCMNotification>();

                    var now = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(3));
                    var today = now.Date;

                    var prayerTimings = await db.PrayerTimings
                        .Include(pt => pt.City)
                        .Where(pt => pt.GregorianDate.Date == today)
                        .ToListAsync(stoppingToken);


                    foreach (var timing in prayerTimings)
                    {
                        var notifyTimes = new[]
                        {
                            (timing.Fajr, "Fajr"),
                            (timing.Dhuhr, "Dhuhr"),
                            (timing.Asr, "Asr"),
                            (timing.Maghrib, "Maghrib"),
                            (timing.Isha, "Isha"),
                        };

                        foreach (var (time, name) in notifyTimes)
                        {
                            if (!DateTimeOffset.TryParseExact(
                                    time,
                                    "HH:mm",
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None,
                                    out var prayerTime))
                            {
                                continue; // skip if invalid
                            }

                            var todayTime = new DateTimeOffset(
                                now.Year, now.Month, now.Day,
                                prayerTime.Hour, prayerTime.Minute, 0,
                                now.Offset
                            );

                            var diff = todayTime - now;


                            // 📝 Log values so you can see why it's skipping
                            _logger.LogInformation(
                                "Now={Now}, Prayer={Prayer}, TodayTime={TodayTime}, DiffSeconds={DiffSeconds}",
                                now, name, todayTime, diff.TotalSeconds
                            );

                            if (diff.TotalSeconds >= 0 && diff.TotalSeconds < 60)
                            {
                                // ✅ Send notification to all devices in this city
                                var devices = await db.Devices
                                    .Where(d => d.Location.Contains(timing.City.CityName))
                                    .Select(d => d.Token)
                                    .ToListAsync(stoppingToken);

                                // 🔍 ADD DEBUG LOGGING HERE
                                _logger.LogInformation("Found {Count} devices for city {City}: {Tokens}",
                                    devices.Count,
                                    timing.City.CityName,
                                    string.Join(", ", devices));

                                _logger.LogInformation("Sending {Count} notifications for {City} - {PrayerName}",
                                    devices.Count, timing.City.CityName, name);


                                foreach (var token in devices)
                                {
                                    try
                                    {
                                        await fcm.SendNotificationAsync(
                                            token,
                                            $"Time for {name} prayer 🕌",
                                            $"It's time for {name} prayer in {timing.City.CityName}."
                                        );

                                        _logger.LogInformation("Notification sent successfully to token: {Token}",
                                            token);
                                    }
                                    catch (FirebaseMessagingException ex) when (ex.MessagingErrorCode ==
                                                                                MessagingErrorCode.Unregistered)
                                    {
                                        // Remove expired/unregistered tokens from database
                                        _logger.LogWarning("Removing expired token: {Token}", token);
                                        var expiredDevice = await db.Devices.FirstOrDefaultAsync(d => d.Token == token);
                                        if (expiredDevice != null)
                                        {
                                            db.Devices.Remove(expiredDevice);
                                        }
                                    }
                                    catch (FirebaseMessagingException ex)
                                    {
                                        _logger.LogError(ex, "FCM error for token: {Token}", token);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex, "Unexpected error sending notification to token: {Token}",
                                            token);
                                    }


                                    _logger.LogInformation("Notification sent successfully to token: {Token}", token);
                                }
                            }
                        }
                    }

                    await db.SaveChangesAsync(stoppingToken);

                    await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // run every minute
                }

                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending scheduled prayer notifications");
                }
            }
        }
    }
}