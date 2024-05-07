
using CsvHelper;
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
            
            var summaries = new[]
            {
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            };
            var lines = await File.ReadAllLinesAsync("ghcnd-stations.txt");
            List<GhcndStations> stations = [.. ParseWeatherData(lines)];
            
            app.MapGet("/weatherforecast", (HttpContext httpContext) =>
            {
                var forecast = Enumerable.Range(1, 5).Select(index =>
                    new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = summaries[Random.Shared.Next(summaries.Length)]
                    })
                    .ToArray();
                return forecast;
            })
            .WithName("GetWeatherForecast")
            .WithOpenApi();

            app.MapGet("/GetMonthlyStatistics", (HttpContext httpContext) =>
            {
                var dayCount = records.Where(x => x.DATE.Month == DateTime.Now.Month).Count(); // Helps protect against missing days of data
                var yearCount = records.Where(x => x.DATE.Month == DateTime.Now.Month).GroupBy(x => x.DATE.Year).Count();

                var avgHigh = records.Where(x => x.DATE.Month == DateTime.Now.Month).Average(x => x.TMAX);
                var avgLow = records.Where(x => x.DATE.Month == DateTime.Now.Month).Average(x => x.TMIN);
                var avgMid = records.Where(x => x.DATE.Month == DateTime.Now.Month).Average(x => x.TAVG);
                var avgWind = records.Where(x => x.DATE.Month == DateTime.Now.Month).Average(x => x.AWND);
                var avgRainPerMonth = records.Where(x => x.DATE.Month == DateTime.Now.Month).Sum(x => x.PRCP) / (double)yearCount;
                var rainyDays = records.Where(x => x.DATE.Month == DateTime.Now.Month).Count(x => x.WT16 == 1) / (double) dayCount;

                return new { AverageHigh = avgHigh, AverageLow = avgLow, AverageMid = avgMid, RainPerMonth = avgRainPerMonth, RainyDays = rainyDays, WindSpeed = avgWind };
            })
            .WithName("GetMonthlyStatistics")
            .WithOpenApi();


            app.Run();
        }

        static IEnumerable<GhcndStations> ParseWeatherData(string[] lines)
        {
            foreach (string line in lines)
            {
                // Parsing based on number of characters
                string stationId = line.Substring(0, 11).Trim();
                double latitude = double.Parse(line.Substring(12, 8).Trim());
                double longitude = double.Parse(line.Substring(21, 9).Trim());
                double elevation = double.Parse(line.Substring(31, 6).Trim());
                string state = line.Substring(38, 2).Trim();
                string stationName = line.Substring(41, 32).Trim();

                // Creating and returning WeatherData object
                yield return new GhcndStations
                {
                    StationId = stationId,
                    Latitude = latitude,
                    Longitude = longitude,
                    Elevation = elevation,
                    State = state,
                    StationName = stationName
                };
            }
        }
    }
}
