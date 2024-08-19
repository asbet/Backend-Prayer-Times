using Backend.DomainModel;

namespace PrayerTimes.DomainModel;

public class Meta: BaseModel
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public required string Timezone { get; set; }
    public required Method Method { get; set; }
    public required string LatitudeAdjustmentMethod { get; set; }
    public required string MidnightMode { get; set; }
    public required string School { get; set; }
    public  required Offset Offset { get; set; }
}

