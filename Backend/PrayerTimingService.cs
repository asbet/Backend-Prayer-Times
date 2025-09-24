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
        var hijriCalendar = new UmAlQuraCalendar();

        foreach (var timing in calendarByCityList.Data)
        {
            // Parse Gregorian safely
            var gregorianDate = DateTimeOffset.ParseExact(
                timing.Date.Gregorian.Date,
                "dd-MM-yyyy",
                CultureInfo.InvariantCulture);

            // Parse Hijri safely
            DateTimeOffset hijriDate;
            try
            {
                var parts = timing.Date.Hijri.Date.Split('-');
                if (parts.Length == 3)
                {
                    int day = int.Parse(parts[0]);
                    int iMonth = int.Parse(parts[1]);
                    int iYear = int.Parse(parts[2]);

                    var hijri = new DateTime(iYear, iMonth, day, hijriCalendar);
                    hijriDate = new DateTimeOffset(hijri);
                }
                else
                {
                    throw new FormatException($"Invalid Hijri date format: {timing.Date.Hijri.Date}");
                }
            }
            catch
            {
                // fallback → match Gregorian if Hijri fails
                hijriDate = gregorianDate;
            }


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
                GregorianDate = DateTimeOffset.ParseExact(timing.Date.Gregorian.Date, "dd-MM-yyyy",
                    CultureInfo.InvariantCulture),
                HijriDate = hijriDate,

                City = city,
            };

            _context.PrayerTimings.Add(prayerTiming);
        }


        await _context.SaveChangesAsync();
    }
}