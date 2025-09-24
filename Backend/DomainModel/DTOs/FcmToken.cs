namespace Backend.DomainModel.DTOs;

public class FcmToken
{
    public int Id { get; set; }

    public required string Token { get; set; }

    public int TokenId { get; set; }

    public PrayerTiming PrayerTiming { get; set; } = null!;
}