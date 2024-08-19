using Backend.DomainModel;

namespace PrayerTimes.DomainModel;

public class Method: BaseModel
{
    public required string Name { get; set; }
    public required Params Params { get; set; }
}
