using Backend.DomainModel;

namespace Backend.Integration.AdhanAPI.Models;
public class Timings
{
    public required string Fajr { get; set; }
    public required string Sunrise { get; set; }
    public required string Dhuhr { get; set; }
    public required string Asr { get; set; }
    public required string Sunset { get; set; }
    public required string Maghrib { get; set; }
    public required string Isha { get; set; }
    public required string Imsak { get; set; }
    public required string Midnight { get; set; }
}
