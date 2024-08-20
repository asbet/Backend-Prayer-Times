using Backend.DomainModel;
using static PrayerTimes.DomainModel.CalendarByCity;

namespace PrayerTimes.DomainModel;

public class Date : BaseModel
{
    public int DateId { get; set; }
    public required string Readable { get; set; }
    public required string Timestamp { get; set; }
    public required Gregorian Gregorian { get; set; }
    public required Hijri Hijri { get; set; }
}
