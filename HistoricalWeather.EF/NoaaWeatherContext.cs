using HistoricalWeather.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HistoricalWeather.EF.Models
{
    public class NoaaWeatherContext(DbContextOptions<NoaaWeatherContext> options) : DbContext(options)
    {
        public string DbPath { get; }

        public DbSet<Station> Stations { get; set; }
        public DbSet<StationDataType> StationDataTypes { get; set; }
        public DbSet<WeatherRecord> WeatherRecords { get; set; }
    }
}
