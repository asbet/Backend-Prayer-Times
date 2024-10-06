using Backend.DomainModel;
using Backend.DomainModel.DTOs;
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
     .HasOne(pt => pt.City)
     .WithMany() 
     .HasForeignKey(pt => pt.CityId) 
     .OnDelete(DeleteBehavior.Cascade);


        base.OnModelCreating(modelBuilder);

    }


    public DbSet<PrayerTiming> PrayerTimings { get; set; }

}
