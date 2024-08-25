using Backend.DomainModel;

namespace Backend.Integration.AdhanAPI.Models;

public class Designation
{
    public required string Abbreviated { get; set; }
    public required string Expanded { get; set; }
}
