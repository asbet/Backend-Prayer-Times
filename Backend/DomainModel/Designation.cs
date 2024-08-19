using Backend.DomainModel;

namespace PrayerTimes.DomainModel;

public class Designation: BaseModel
{
    public required string Abbreviated { get; set; }
    public required string Expanded { get; set; }
}
