using Backend.DomainModel;
using static PrayerTimes.DomainModel.CalendarByCity;

namespace PrayerTimes.DomainModel;

public class Datum: BaseModel
{
    public int CalendarByCityId { get; set; } 
    public required Timings Timings { get; set; }
    public required Date Date { get; set; }
    public required Meta Meta { get; set; }
}
    