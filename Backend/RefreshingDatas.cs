using Backend.DomainModel;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Xml;

namespace Backend;

public class RefreshingDatas
{
    private DateTime LastRefreshTime;

    private readonly IServiceScopeFactory ScopeFactory;

    public RefreshingDatas(IServiceScopeFactory scopeFactory)
    {
        ScopeFactory = scopeFactory;
    }
    public async Task<IEnumerable<PrayerTiming>> Save()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        LastRefreshTime = configuration.GetValue<DateTime>("LastRefreshTime");

        if (DateTime.Now - LastRefreshTime > TimeSpan.FromMinutes(3))
        {
            using (var scope = ScopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<PrayerTimingService>();
                var countries = await service.SavePrayerTimingsAsync();


                Console.WriteLine(JsonConvert.SerializeObject(countries, Formatting.Indented));

                LastRefreshTime = DateTime.Now;

                File.WriteAllText("appsettings.json",
                    JsonConvert.SerializeObject(new { LastRefreshTime = LastRefreshTime }, Formatting.Indented));
            }
        }
    }
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

}
