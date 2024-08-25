using Backend.DomainModel;

namespace Backend.Integration.AdhanAPI.Models;

public class Meta
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public required string Timezone { get; set; }
    public required Method Method { get; set; }
    public required string LatitudeAdjustmentMethod { get; set; }
    public required string MidnightMode { get; set; }
    public required string School { get; set; }
    public required Offset Offset { get; set; }
}

