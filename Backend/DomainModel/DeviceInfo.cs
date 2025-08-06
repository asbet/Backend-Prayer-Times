using System.ComponentModel.DataAnnotations;

namespace Backend.DomainModel;

public class DeviceInfo
{
    public int Id { get; set; }

    [MaxLength(1500)] public string Token { get; set; } = string.Empty;

    [MaxLength(250)] public string Platform { get; set; } = string.Empty;

    [MaxLength(250)] public string Brand { get; set; } = string.Empty;

    [MaxLength(250)] public string Model { get; set; } = string.Empty;

    [MaxLength(250)] public string Version { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;


    [MaxLength(150)] public required string Location { get; set; }
}