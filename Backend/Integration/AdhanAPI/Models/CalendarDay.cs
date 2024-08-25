using Backend.DomainModel;
using static Backend.Integration.AdhanAPI.Models.CalendarByCity;

namespace Backend.Integration.AdhanAPI.Models;

public class CalendarDay
{
    public int CalendarByCityId { get; set; }
    public required Timings Timings { get; set; }
    public required Date Date { get; set; }
    public required Meta Meta { get; set; }
}
