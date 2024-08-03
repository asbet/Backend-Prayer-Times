namespace PrayerTimes.DomainModel;

public class CalendarByCity
{
    // http://api.aladhan.com/v1/calendarByCity/:year/:month
    public required List<Datum> Data { get; set; }

}

