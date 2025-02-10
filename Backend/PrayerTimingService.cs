using System.Globalization;
using Backend.DomainModel;
using Backend.DomainModel.DTOs;
using Backend.Integration.AdhanAPI;

namespace Backend;

public class PrayerTimingService
{
    private readonly PrayerTimesDbContext _context;

    public PrayerTimingService(PrayerTimesDbContext context)
    {
        _context = context;
    }
    public async Task SavePrayerTimingsAsync(CalendarByCity calendarByCityList, City city)
    {
        foreach (var timing in calendarByCityList.Data)
        {
            var prayerTiming = new PrayerTiming
            {


                Fajr = timing.Timings.Fajr,
                Imsak = timing.Timings.Imsak,
                Sunrise = timing.Timings.Sunrise,
                Dhuhr = timing.Timings.Dhuhr,
                Asr = timing.Timings.Asr,
                Sunset = timing.Timings.Sunset,
                Maghrib = timing.Timings.Maghrib,
                Isha = timing.Timings.Isha,
                Midnight = timing.Timings.Midnight,
                GregorianDate = DateTimeOffset.ParseExact(timing.Date.Gregorian.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture),
                HijriDate = DateTimeOffset.ParseExact(timing.Date.Hijri.Date, "dd-MM-yyyy", CultureInfo.InvariantCulture),

                City = city,
               
            };

            _context.PrayerTimings.Add(prayerTiming);

        }


        await _context.SaveChangesAsync();
    }
}
