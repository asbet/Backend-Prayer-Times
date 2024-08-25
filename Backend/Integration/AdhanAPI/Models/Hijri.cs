using Backend.DomainModel;

namespace Backend.Integration.AdhanAPI.Models;

public class Hijri
{
    public int? HijriId { get; set; }
    public required string Date { get; set; }
    public required string Format { get; set; }
    public required string Day { get; set; }
    public required Weekday Weekday { get; set; }
    public required Month Month { get; set; }
    public required string Year { get; set; }
    public required Designation Designation { get; set; }
    public required List<string> Holidays { get; set; }
}
