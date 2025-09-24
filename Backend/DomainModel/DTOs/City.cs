using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.DomainModel.DTOs;

public class City
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CityId { get; set; }

    [MaxLength(1500)] public required string CityName { get; set; }
    [MaxLength(1500)] public required string CountryName { get; set; }

    public ICollection<PrayerTiming> PrayerTimings { get; set; } = new List<PrayerTiming>();
}