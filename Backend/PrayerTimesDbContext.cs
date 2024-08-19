using Microsoft.EntityFrameworkCore;
using PrayerTimes.DomainModel;

namespace Backend;

public class PrayerTimesDbContext : DbContext
{
    //todo search about the migration
    public PrayerTimesDbContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CalendarByCity>()
            .HasMany(c => c.Data)
            .WithOne()
            .HasForeignKey(d => d.CalendarByCityId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Datum>()
            .HasOne(d => d.Timings)
            .WithOne()
            .HasForeignKey<Datum>(d=>d.Id)
            .OnDelete(DeleteBehavior.Cascade);
         
        modelBuilder.Entity<Datum>()
            .HasOne(d => d.Date)
            .WithOne()
            .HasForeignKey<Datum>(d=>d.Id)
            .OnDelete(DeleteBehavior.Cascade); 
        
        modelBuilder.Entity<Datum>()
            .HasOne(d => d.Meta)
            .WithOne()
            .HasForeignKey<Datum>(d=>d.Id)
            .OnDelete(DeleteBehavior.Cascade);

    }


    public DbSet<CalendarByCity> CalendarByCities { get; set; }

    public DbSet<Timings> Timings { get; set; }

    public DbSet<Date> Dates { get; set; }

    public DbSet<Meta> Metas { get; set; }
}
