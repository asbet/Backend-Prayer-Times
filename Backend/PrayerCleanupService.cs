using Microsoft.EntityFrameworkCore;

namespace Backend;

public class PrayerCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PrayerCleanupService> _logger;

    public PrayerCleanupService(IServiceProvider serviceProvider, ILogger<PrayerCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PrayerTimesDbContext>();

            var cutoff = DateTimeOffset.UtcNow.Date.AddDays(-3); // keep last 3 days
            var oldEntries = await db.PrayerTimings
                .Where(pt => pt.GregorianDate < cutoff)
                .ToListAsync(stoppingToken);

            if (oldEntries.Any())
            {
                db.PrayerTimings.RemoveRange(oldEntries);
                await db.SaveChangesAsync(stoppingToken);
                _logger.LogInformation("🗑️ Removed {Count} old prayer timings (before {Cutoff})", oldEntries.Count,
                    cutoff);
            }

            await Task.Delay(TimeSpan.FromHours(24), stoppingToken); // run once a day
        }
    }
}