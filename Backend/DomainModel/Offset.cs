using Backend.DomainModel;

namespace PrayerTimes.DomainModel;

public class Offset: BaseModel
{
    public int Imsak { get; set; }
    public int Fajr { get; set; }
    public int Sunrise { get; set; }
    public int Dhuhr { get; set; }
    public int Asr { get; set; }
    public int Maghrib { get; set; }
    public int Sunset { get; set; }
    public int Isha { get; set; }
    public int Midnight { get; set; }
}
