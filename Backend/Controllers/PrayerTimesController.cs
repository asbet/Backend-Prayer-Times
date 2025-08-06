using System.Globalization;
using Backend.Configuration;
using Backend.DomainModel.DTOs;
using Backend.Integration.AdhanAPI;
using Backend.Notification;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/[controller]")]
public class PrayerTimesController(
    CheckExistDatas checkExistDatas,
    IPrayerTimesServices services,
    PrayerTimingService prayerTimingService,
    ILogger<PrayerTimesController> logger)
    : ControllerBase
{
    [HttpGet("{year}/{month}/{city}/{country}")]
    public async Task<ActionResult<CalendarByCity>> GetPrayerTimes(int year, int month, string city, string country,
        [FromQuery] int method)
    {
        if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
        {
            return BadRequest("City and country are required.");
        }

        try
        {
            var prayerTimes = await services.GetTimes(year, month, city, country, method);

            // City selectedCity22 = new City { CityName = city, CountryName = country };
            
            var selectedCity = await checkExistDatas.GetOrCreateCityAsync(city, country);

            if (await checkExistDatas.IsPrayerTimingAlreadySavedAsync(year, month, selectedCity))
            {
                return Conflict("Prayer times for this city and month already exist.");
            }

            await prayerTimingService.SavePrayerTimingsAsync(prayerTimes, selectedCity);

            PrayerTiming dto = null;
            foreach (var times in prayerTimes.Data)
            {
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
                    GregorianDate = DateTimeOffset.ParseExact(times.Date.Gregorian.Date, "dd-MM-yyyy",
                        CultureInfo.InvariantCulture),
                    HijriDate = DateTimeOffset.ParseExact(times.Date.Hijri.Date, "dd-MM-yyyy",
                        CultureInfo.InvariantCulture),
                };
            }

            return Ok(dto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred while fetching prayer times for {City}, {Country}.", city, country);

            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}