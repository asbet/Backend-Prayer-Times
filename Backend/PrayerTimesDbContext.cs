using Backend.DomainModel;
using Microsoft.EntityFrameworkCore;

namespace Backend;

public class PrayerTimesDbContext : DbContext
{
    public PrayerTimesDbContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PrayerTiming>()
            .HasOne(c => c.City)
            .WithOne()
            .HasForeignKey<City>(d => d.CityId)
            .OnDelete(DeleteBehavior.Cascade);

        base.OnModelCreating(modelBuilder);

    }


    public DbSet<PrayerTiming> PrayerTimings { get; set; }

}
