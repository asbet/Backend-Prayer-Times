using Backend.DomainModel;
using Backend.Integration.AdhanAPI;

namespace Backend;

public class PrayerTimingService
{
    private readonly PrayerTimesDbContext _context;

    public PrayerTimingService(PrayerTimesDbContext context)
    {
        _context = context;
    }
    public async Task SavePrayerTimingsAsync(CalendarByCity calendarByCityList, String city)
    {
            foreach (var timing in calendarByCityList.Data)
            {
            var prayerTiming = new PrayerTiming
                {
                
         
                    Fajr = timing.Timings.Fajr,
                    Sunrise = timing.Timings.Sunrise,
                    Dhuhr = timing.Timings.Dhuhr,
                    Asr = timing.Timings.Asr,
                    Sunset = timing.Timings.Sunset,
                    Maghrib = timing.Timings.Maghrib,
                    Isha = timing.Timings.Isha,
                    Imsak = timing.Timings.Imsak,
                    Midnight = timing.Timings.Midnight,
                    GregorianDate = DateTimeOffset.Parse(timing.Date.Gregorian.Date),
                    HijriDate = DateTimeOffset.Parse(timing.Date.Gregorian.Date),
                    City = null,
                  
                };

                _context.PrayerTimings.Add(prayerTiming);
           
            }
        

        await _context.SaveChangesAsync();
    }
}
