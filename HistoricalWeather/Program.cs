
using CsvHelper;
using HistoricalWeather.Models;
using HistoricalWeather.Services;
using System.Globalization;
using System.Linq;

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
            builder.Services.AddScoped<StationRecordService>(); 

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

            List<DailyWeather> records;

            using (var reader = new StreamReader("3683187.csv"))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                records = csv.GetRecords<DailyWeather>().ToList();
            }

            var lines = await File.ReadAllLinesAsync("ghcnd-stations.txt");
            IEnumerable<GhcndStation> stations = [.. StationService.ParseStationData(lines)];

            GhcndStation closestStation = StationService.FindClosestStation(stations, 37.327000, -121.915700);

            var stationInventorylines = await File.ReadAllLinesAsync("ghcnd-inventory.txt");
            IEnumerable<GhcndIndex> stations2 = [.. StationService.ParseStationIndexData(stationInventorylines)];

            var filePath = $"\\ghcnd_all\\{closestStation.StationId}.dly";
            var lines2 = await File.ReadAllLinesAsync(filePath);

            var t = StationRecordService.ParseFile(lines2);
            var y = t.GroupBy(x => x.Element);
            
            app.MapGet("/GetMonthlyStatistics", (HttpContext httpContext) =>
            {
                var dayCount = records.Where(x => x.DATE.Month == DateTime.Now.Month).Count(); // Helps protect against missing days of data
                var yearCount = records.Where(x => x.DATE.Month == DateTime.Now.Month).GroupBy(x => x.DATE.Year).Count();

                var avgHigh = records.Where(x => x.DATE.Month == DateTime.Now.Month).Average(x => x.TMAX);
                var avgLow = records.Where(x => x.DATE.Month == DateTime.Now.Month).Average(x => x.TMIN);
                var avgMid = records.Where(x => x.DATE.Month == DateTime.Now.Month).Average(x => x.TAVG);
                var avgWind = records.Where(x => x.DATE.Month == DateTime.Now.Month).Average(x => x.AWND);
                var avgRainPerMonth = records.Where(x => x.DATE.Month == DateTime.Now.Month).Sum(x => x.PRCP) / (double)yearCount;
                var rainyDays = records.Where(x => x.DATE.Month == DateTime.Now.Month).Count(x => x.WT16 == 1) / (double)dayCount;

                return new { AverageHigh = avgHigh, AverageLow = avgLow, AverageMid = avgMid, RainPerMonth = avgRainPerMonth, RainyDays = rainyDays, WindSpeed = avgWind };
            })
            .WithName("GetMonthlyStatistics")
            .WithOpenApi();

            app.Run();
        }
    }
}
