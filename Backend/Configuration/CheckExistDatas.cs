using Backend.DomainModel.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Backend.Configuration;

public class CheckExistDatas
{
    private readonly PrayerTimesDbContext _dbContext;

    public CheckExistDatas(PrayerTimesDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<City> GetOrCreateCityAsync(string cityName, string countryName)
    {
        var city = await _dbContext.Set<City>()
            .FirstOrDefaultAsync(c => c.CityName == cityName && c.CountryName == countryName);

        if (city == null)
        {
            city = new City { CityName = cityName, CountryName = countryName };
            _dbContext.Add(city);
            await _dbContext.SaveChangesAsync();
        }

        return city;
    }

    public async Task<bool> IsPrayerTimingAlreadySavedAsync(int year, int month, City city)
    {
        return await _dbContext.PrayerTimings
            .AnyAsync(pt =>
                pt.City.CityName == city.CityName &&
                pt.City.CountryName == city.CountryName &&
                pt.GregorianDate.Year == year &&
                pt.GregorianDate.Month == month);
    }
}