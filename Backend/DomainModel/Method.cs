namespace PrayerTimes.DomainModel;

public class Method
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required Params Params { get; set; }
}
