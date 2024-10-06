namespace Backend.DomainModel.DTOs;

public class City
{
    public int Id { get; set; }
    public required string CityName { get; set; }
    public required string CountryName { get; set; }
}
