using HistoricalWeather.Api.Services;
using HistoricalWeather.Domain.Models;
using HistoricalWeather.EF;
using Microsoft.EntityFrameworkCore;

namespace HistoricalWeather.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddScoped<StationService>();
            builder.Services.AddDbContext<NoaaWeatherContext>(options => options.UseSqlServer(builder.Configuration["ConnectionString"]));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            DbContextOptionsBuilder dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseSqlServer();

            string[] lines = await File.ReadAllLinesAsync("ghcnd-stations.txt");
            IEnumerable<Station> stations = StationService.ParseStationData(lines);

            Station closestStation = StationService.FindClosestStation(stations, 37.327000, -121.915700);

            Environment.SpecialFolder folder = Environment.SpecialFolder.LocalApplicationData;
            string path = Environment.GetFolderPath(folder);

            string[] stationInventorylines = await File.ReadAllLinesAsync("ghcnd-inventory.txt");
            IEnumerable<StationDataType> stations2 = StationService.ParseStationIndexData(stationInventorylines);

            string filePath = $"\\ghcnd_all\\{closestStation.Id}.dly";
            string[] lines2 = await File.ReadAllLinesAsync(filePath);

            app.Run();
        }
    }
}
