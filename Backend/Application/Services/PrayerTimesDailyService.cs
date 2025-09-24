using System.Globalization;
using Backend.Configuration;
using Backend.DomainModel.DTOs;
using Backend.Integration.AdhanAPI;

namespace Backend.Application.Services;

public class PrayerTimesDailyService
{
    private readonly CheckExistDatas _checkExistDatas;
    private readonly IPrayerTimesServices _services;
    private readonly PrayerTimingService _prayerTimingService;
    private readonly ILogger<PrayerTimesDailyService> _logger;

    public PrayerTimesDailyService(
        CheckExistDatas checkExistDatas,
        IPrayerTimesServices services,
        PrayerTimingService prayerTimingService,
        ILogger<PrayerTimesDailyService> logger)
    {
        _checkExistDatas = checkExistDatas;
        _services = services;
        _prayerTimingService = prayerTimingService;
        _logger = logger;
    }

    public async Task<PrayerTiming?> SendDailyPrayerTimesAsync(
        int year, int month, string city, string country, int method)
    {
        if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
        {
            throw new ArgumentException("City and country are required.");
        }

        try
        {
            var prayerTimes = await _services.GetTimes(year, month, city, country, method);
            var selectedCity = await _checkExistDatas.GetOrCreateCityAsync(city, country);

            if (!await _checkExistDatas.IsPrayerTimingAlreadySavedAsync(year, month, selectedCity))
            {
                await _prayerTimingService.SavePrayerTimingsAsync(prayerTimes, selectedCity);
            }

            PrayerTiming? dto = null;
            var hijriCalendar = new UmAlQuraCalendar();

            foreach (var times in prayerTimes.Data)
            {
                DateTimeOffset? hijriDate = null;
                try
                {
                    var parts = times.Date.Hijri.Date.Split('-');
                    if (parts.Length == 3)
                    {
                        int day = int.Parse(parts[0]);
                        int iMonth = int.Parse(parts[1]);
                        int iYear = int.Parse(parts[2]);

                        var date = new DateTime(iYear, iMonth, day, hijriCalendar);
                        hijriDate = new DateTimeOffset(date);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Invalid Hijri date: {Date}", times.Date.Hijri.Date);
                }

                dto = new PrayerTiming
                {
                    Sunrise = times.Timings.Sunrise,
                    Fajr = times.Timings.Fajr,
                    City = selectedCity,
                    Dhuhr = times.Timings.Dhuhr,
                    Asr = times.Timings.Asr,
                    Sunset = times.Timings.Sunset,
                    Maghrib = times.Timings.Maghrib,
                    Isha = times.Timings.Isha,
                    Imsak = times.Timings.Imsak,
                    Midnight = times.Timings.Midnight,
                    GregorianDate = DateTimeOffset.ParseExact(
                        times.Date.Gregorian.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                    HijriDate = hijriDate
                };
            }

            return dto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching prayer times for {City}, {Country}", city, country);
            throw; // let controller handle the exception
        }
    }
}