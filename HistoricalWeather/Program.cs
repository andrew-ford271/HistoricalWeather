using HistoricalWeather.Domain.Models;
using HistoricalWeather.EF.Models;
using HistoricalWeather.Services;

namespace HistoricalWeather
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddScoped<StationService>();
            builder.Services.AddDbContext<NoaaWeatherContext>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            using var db = new NoaaWeatherContext();

            var lines = await File.ReadAllLinesAsync("ghcnd-stations.txt");
            IEnumerable<Station> stations = StationService.ParseStationData(lines);

            Station closestStation = StationService.FindClosestStation(stations, 37.327000, -121.915700);

            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);

            db.Add(closestStation);
            db.SaveChanges();
            var g = db.Stations.ToList();

            var stationInventorylines = await File.ReadAllLinesAsync("ghcnd-inventory.txt");
            IEnumerable<StationDataType> stations2 = StationService.ParseStationIndexData(stationInventorylines);

            var filePath = $"\\ghcnd_all\\{closestStation.Id}.dly";
            var lines2 = await File.ReadAllLinesAsync(filePath);


            app.Run();
        }
    }
}
