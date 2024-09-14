namespace Backend.DomainModel
{
    public class PrayerTiming
    {
        public  int Id { get; set; }
        public required string Fajr { get; set; }
        public required string Sunrise { get; set; }
        public required string Dhuhr { get; set; }
        public required string Asr { get; set; }
        public required string Sunset { get; set; }
        public required string Maghrib { get; set; }
        public required string Isha { get; set; }
        public required string Imsak { get; set; }
        public required string Midnight { get; set; }
        public required DateTimeOffset GregorianDate { get; set; }
        public required DateTimeOffset HijriDate { get; set; }
        public required City City { get; set; }
        public  int? CityId { get; set; }
    }
}
