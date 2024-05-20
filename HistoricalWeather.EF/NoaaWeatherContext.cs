using HistoricalWeather.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HistoricalWeather.EF.Models
{
    public class NoaaWeatherContext : DbContext
    {
        public string DbPath { get; }

        public NoaaWeatherContext()
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlServer($"");

        public DbSet<Station> Stations { get; set; }
        public DbSet<StationDataType> StationDataTypes { get; set; }
        public DbSet<WeatherRecord> WeatherRecords { get; set; }
    }
}
