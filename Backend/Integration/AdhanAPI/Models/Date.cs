using Backend.DomainModel;
using static Backend.Integration.AdhanAPI.Models.CalendarByCity;

namespace Backend.Integration.AdhanAPI.Models;

public class Date
{
  
    public required string Readable { get; set; }
    public required string Timestamp { get; set; }
    public required Gregorian Gregorian { get; set; }
    public required Hijri Hijri { get; set; }
}
