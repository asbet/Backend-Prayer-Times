using Backend.DomainModel.DTOs;
using Backend.Integration.AdhanAPI;
using Backend.Notification;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/[controller]")]
public class PrayerTimesController(
    IPrayerTimesServices services,
    PrayerTimingService prayerTimingService,
    ILogger<PrayerTimesController> logger,
    FCMNotification notificationService)
    : ControllerBase
{


    [HttpGet("{year}/{month}")]
    public async Task<ActionResult<CalendarByCity>> GetPrayerTimes(int year, int month, [FromQuery] string city, [FromQuery] string country, [FromQuery] int method)
    {
        if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(country))
        {
            return BadRequest("City and country are required.");
        }

        try
        {
            var prayerTimes = await services.GetTimes(year, month, city, country, method);

            City selectedCity= new City{CityName=city,CountryName=country};

            await prayerTimingService.SavePrayerTimingsAsync(prayerTimes, selectedCity);
            PrayerTiming dto = null;
            foreach (var times in prayerTimes.Data)
            {
                dto = new PrayerTiming
                {

                    Sunrise = times.Timings.Sunrise,
                    Fajr = times.Timings.Fajr,
                    City=selectedCity,
                    Dhuhr = times.Timings.Dhuhr,
                    Asr = times.Timings.Asr,
                    Sunset = times.Timings.Sunset,
                    Maghrib = times.Timings.Maghrib,
                    Isha = times.Timings.Isha,
                    Imsak = times.Timings.Imsak,
                    Midnight = times.Timings.Midnight,
                    GregorianDate = DateTimeOffset.Parse(times.Date.Gregorian.Date),
                    HijriDate = DateTimeOffset.Parse(times.Date.Hijri.Date),

                };
            }
            await notificationService.SendFCMNotification(dto.Asr, dto.Asr, "");

            return Ok(dto);

        }
        catch (Exception ex)
        {

            logger.LogError(ex, "An error occurred while fetching prayer times for {City}, {Country}.", city, country);

            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}