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
            .WithOne(ct => ct.PrayerTiming)
            .HasForeignKey<City>(pt => pt.CityId) 
            .IsRequired();
        
        
        modelBuilder.Entity<PrayerTiming>()
            .HasMany(tkn => tkn.FcmTokens)
            .WithOne(tkn => tkn.PrayerTiming)
            .HasForeignKey(pt => pt.TokenId) 
            .IsRequired();


        base.OnModelCreating(modelBuilder);

    }


    public DbSet<PrayerTiming> PrayerTimings { get; set; }

}
