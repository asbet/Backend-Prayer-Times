using Refit;

namespace Backend.Integration.AdhanAPI
{
    public interface IPrayerTimesServices
    {
        //http://api.aladhan.com/v1/calendarByAddress/:year/:month
        //http://api.aladhan.com/v1/timingsByCity/{date}
        //http://api.aladhan.com/v1/calendarByCity/2017/4?city=London&country=United%20Kingdom&method=2
        [Get("/v1/calendarByCity/{year}/{month}?city={city}&country={country}&method={method}")]
        Task<CalendarByCity> GetTimes(
        [AliasAs("year")] int year,
        [AliasAs("month")] int month,
        [AliasAs("city")] string city,
        [AliasAs("country")] string country,
        [AliasAs("method")] int method
            );
    }
}