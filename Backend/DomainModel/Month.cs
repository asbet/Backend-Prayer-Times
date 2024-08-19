using Backend.DomainModel;

namespace PrayerTimes.DomainModel;

public class Month: BaseModel
{
    public required int Number { get; set; }
    public required string En { get; set; }
    public required string Ar { get; set; }
}
