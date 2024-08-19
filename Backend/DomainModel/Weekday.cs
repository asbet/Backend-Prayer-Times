using Backend.DomainModel;

namespace PrayerTimes.DomainModel;

public class Weekday: BaseModel
{
    public required string En { get; set; }
    public required string Ar { get; set; }
}
