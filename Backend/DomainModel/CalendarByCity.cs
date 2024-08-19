using Backend.DomainModel;

namespace PrayerTimes.DomainModel;

public class CalendarByCity: BaseModel
{
    public required List<Datum> Data { get; set; }

}

