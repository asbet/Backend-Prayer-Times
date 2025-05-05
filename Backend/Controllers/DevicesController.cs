using Backend.DomainModel;
using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DevicesController : ControllerBase
{
    private readonly PrayerTimesDbContext _db;

    public DevicesController(PrayerTimesDbContext db)
    {
        _db = db;
    }

    [HttpPost]
    [SuppressMessage("ReSharper.DPA", "DPA0011: High execution time of MVC action", MessageId = "time: 1157ms")]
    public async Task<IActionResult> RegisterDevice([FromBody] DeviceInfo info)
    {
        if (string.IsNullOrWhiteSpace(info.Token)) return BadRequest("Token required");

        var existing = await _db.Devices.FirstOrDefaultAsync(d => d.Token == info.Token);
        if (existing != null)
        {
            existing.Brand = info.Brand;
            existing.Model = info.Model;
            existing.Version = info.Version;
            existing.Platform = info.Platform;
            existing.RegisteredAt = DateTime.UtcNow;
        }
        else
        {
            await _db.Devices.AddAsync(info);
        }

        await _db.SaveChangesAsync();
        return Ok("✅ Device saved successfully");
    }
}

