

using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class NewTestController:ControllerBase
{
    [HttpGet]
    public IActionResult Index()
    {
        return Ok("Successful Connected bu ikinci deneme");
    }
}
