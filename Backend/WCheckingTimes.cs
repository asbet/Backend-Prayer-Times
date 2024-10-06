
using Backend.DomainModel.DTOs;
using Backend.Integration.AdhanAPI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.Metrics;
using System.Text.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Backend;

public class WCheckingTimes : BackgroundService
{
    private readonly ILogger<WCheckingTimes> _logger;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly HttpClient _httpClient;
    private readonly IPrayerTimesServices _prayerTimesServices;



    public WCheckingTimes(ILogger<WCheckingTimes> logger, IServiceScopeFactory scopeFactory, HttpClient httpClient, IPrayerTimesServices prayerTimesServices)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _httpClient = httpClient;
        _prayerTimesServices = prayerTimesServices;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromDays(1), stoppingToken);


            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<PrayerTimesDbContext>();

                var missingTimes = await context.PrayerTimings
                        .Where(b => string.IsNullOrEmpty(b.Sunset) || string.IsNullOrEmpty(b.Imsak))
                        .Select(b => b.City)
                        .ToListAsync(stoppingToken);

                var missingCountry = await context.PrayerTimings
                 .Select(country => country.City.CountryName).ToListAsync(stoppingToken);

                if (missingTimes.Any())
                {
                    foreach (var timings in missingTimes)
                    {

                        var calendarData = await _prayerTimesServices.GetTimes(DateTimeOffset.Now.DayOfYear, DateTimeOffset.Now.Month, timings.CityName, timings.CountryName, 0);
                       
                        
                        var prayerTiming = await context.PrayerTimings
                            .FirstOrDefaultAsync(t => t.City.CityName == timings.CityName && t.City.CountryName == timings.CountryName, stoppingToken);

                        if (prayerTiming != null)
                        {
                            
                            var apiTiming = calendarData.Data.FirstOrDefault(); 
                            if (apiTiming != null)
                            {
                                prayerTiming.Fajr = apiTiming.Timings.Fajr;
                                prayerTiming.Dhuhr = apiTiming.Timings.Dhuhr;
                                prayerTiming.Asr = apiTiming.Timings.Asr;
                                prayerTiming.Maghrib = apiTiming.Timings.Maghrib;
                                prayerTiming.Isha = apiTiming.Timings.Isha;
                                prayerTiming.Sunset = apiTiming.Timings.Sunset; 
                                prayerTiming.Imsak = apiTiming.Timings.Imsak;   
                                prayerTiming.City=new City { CityName = timings.CityName, CountryName = timings.CountryName };
                            }
                        }

                    }

                    await context.SaveChangesAsync(stoppingToken);
                }
                var now = DateTime.Now;
                var nextRun = now.Date.AddDays(1);
                var delay = nextRun - now;

                await Task.Delay(delay, stoppingToken);
            }
        }

    }

}
