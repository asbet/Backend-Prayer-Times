using Backend.DomainModel;
using Backend.Integration.AdhanAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrayerTimesController : ControllerBase
    {
        IPrayerTimesServices services;

        public PrayerTimesController(IPrayerTimesServices services)
        {
            this.services = services;
        }

        [HttpGet("{year}/{month}")]
        public async Task<IActionResult> GetPrayerTimes(int year, int month, [FromQuery] string city, [FromQuery] string country, [FromQuery] int method)
        {
            try
            {
                var prayerTimes = await services.GetTimes(year, month, city, country, method);

                return Ok(prayerTimes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while fetching prayer times.");
            }
        }
    }
}
