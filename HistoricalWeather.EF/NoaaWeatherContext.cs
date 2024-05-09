using HistoricalWeather.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace HistoricalWeather.EF.Models
{
    public class NoaaWeatherContext : DbContext
    {
        public string DbPath { get; }
        
        public NoaaWeatherContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = Path.Join(path, "NoaaWeather.db");

        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");

        public DbSet<Station> Stations { get; set; }
        public DbSet<StationDataType> StationDataTypes { get; set; }
        public DbSet<WeatherRecordMonth> WeatherRecordMonths { get; set; }
        public DbSet<WeatherRecordDay> WeatherRecordDays { get; set; }
    }
}
