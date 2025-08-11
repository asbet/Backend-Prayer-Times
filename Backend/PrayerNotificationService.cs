using System.Globalization;
using Backend.Notification;
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

                    var now = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(3)); // or adjust to device TZ
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
                            // Parse string "HH:mm" to today's DateTimeOffset with the same hour and minute
                            var parsedTime = DateTimeOffset.TryParseExact(
                                time,
                                "HH:mm",
                                CultureInfo.InvariantCulture,
                                DateTimeStyles.None,
                                out var prayerTime
                            )
                                ? prayerTime
                                : DateTimeOffset.MinValue;

                            if (parsedTime == DateTimeOffset.MinValue) continue; // skip if invalid

                            var todayTime = new DateTimeOffset(
                                now.Year, now.Month, now.Day,
                                prayerTime.Hour, prayerTime.Minute, 0,
                                now.Offset // keep same timezone offset as now
                            );

                            var diff = todayTime - now;
                            if (diff.TotalMinutes > 0 && diff.TotalMinutes < 1)
                            {
                                // ✅ Send notification to all devices in this city
                                var devices = await db.Devices
                                    .Where(d => d.Location == timing.City.CityName)
                                    .Select(d => d.Token) // only get what you need
                                    .ToListAsync(stoppingToken);

                                foreach (var token in devices)
                                {
                                    await fcm.SendNotificationAsync(
                                        token,
                                        $"Time for {name} prayer 🕌",
                                        $"It's time for {name} prayer in {timing.City.CityName}."
                                    );
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