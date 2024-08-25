using Backend.Integration.AdhanAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend;

public class PrayerTimesDbContext : DbContext
{
    public PrayerTimesDbContext(DbContextOptions options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CalendarByCity>()
            .HasMany(c => c.Data)
            .WithOne()
            .HasForeignKey(d => d.CalendarByCityId)
            .OnDelete(DeleteBehavior.NoAction); 

        modelBuilder.Entity<CalendarDay>()
            .HasOne(d => d.Timings)
            .WithOne()
            .HasForeignKey<Timings>(d=>d.TimingsId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<CalendarDay>()
            .HasOne(d => d.Date)
            .WithOne()
            .HasForeignKey<Date>(d=>d.DateId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CalendarDay>()
            .HasOne(d => d.Meta)
            .WithOne()
            .HasForeignKey<Meta>(d=>d.MetaId)
            .OnDelete(DeleteBehavior.NoAction);

        base.OnModelCreating(modelBuilder);

    }


    public DbSet<CalendarByCity> CalendarByCities { get; set; }

    public DbSet<Timings> Timings { get; set; }

    public DbSet<Date> Dates { get; set; }

    public DbSet<Meta> Metas { get; set; }
}
