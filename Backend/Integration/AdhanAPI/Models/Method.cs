using Backend.DomainModel;

namespace Backend.Integration.AdhanAPI.Models;

public class Method 
{
    public required string Name { get; set; }
    public required Params Params { get; set; }
}
