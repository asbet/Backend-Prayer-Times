using Backend.Integration.AdhanAPI;
using Backend;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
public class PrayerTimesController : ControllerBase
{
    private readonly IPrayerTimesServices services;
    private readonly PrayerTimingService prayerTimingService;
    private readonly ILogger<PrayerTimesController> logger;

    public PrayerTimesController(IPrayerTimesServices services,
                                 PrayerTimingService prayerTimingService,
                                 ILogger<PrayerTimesController> logger)
    {
        this.services = services;
        this.prayerTimingService = prayerTimingService;
        this.logger = logger;
    }

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
            await prayerTimingService.SavePrayerTimingsAsync(prayerTimes, city);

            return Ok(prayerTimes);
        }
        catch (Exception ex)
        {
           
            logger.LogError(ex, "An error occurred while fetching prayer times for {City}, {Country}.", city, country);

            
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}
