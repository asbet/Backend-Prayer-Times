using Backend.Application.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DailyPrayerTimesController : ControllerBase
{
    private readonly PrayerTimesDailyService _dailyService;

    public DailyPrayerTimesController(PrayerTimesDailyService dailyService)
    {
        _dailyService = dailyService;
    }

    [HttpGet("today/{city}/{country}")]
    public async Task<IActionResult> GetDaily(string city, string country, [FromQuery] int method = 2)
    {
        try
        {
            var now = DateTime.UtcNow;
            var result = await _dailyService.SendDailyPrayerTimesAsync(now.Year, now.Month, city, country, method);

            if (result == null)
                return NotFound("No prayer times found.");

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred: {ex.Message}");
        }
    }
}