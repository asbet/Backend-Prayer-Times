using Backend.DomainModel;
using static Backend.Integration.AdhanAPI.Models.CalendarByCity;

namespace Backend.Integration.AdhanAPI.Models;

public class Gregorian 
{
    public required string Date { get; set; }
    public required string Format { get; set; }
    public required string Day { get; set; }
    public required Weekday Weekday { get; set; }
    public required Month Month { get; set; }
    public required string Year { get; set; }
    public required Designation Designation { get; set; }
}