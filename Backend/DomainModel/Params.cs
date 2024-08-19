using Backend.DomainModel;

namespace PrayerTimes.DomainModel;

public class Params: BaseModel
{
    public int Fajr { get; set; }
    public int Isha { get; set; }
}
