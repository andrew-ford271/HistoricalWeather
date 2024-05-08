
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
            

            var lines = await File.ReadAllLinesAsync("ghcnd-stations.txt");
            List<GhcndStation> stations = [.. ParseWeatherData(lines)];

            GhcndStation closestStation = FindClosestStation(stations, 37.327000, -121.915700);

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

        protected static GhcndStation FindClosestStation(List<GhcndStation> ghcndStations, double targetLatitude, double targetLongitude)
        {
            GhcndStation closestStation = ghcndStations[0];
            double minDistance = double.MaxValue;

            foreach (var station in ghcndStations)
            {
                double distance = CalculateDistance(targetLatitude, targetLongitude, station.Latitude, station.Longitude);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestStation = station;
                }
            }


            return closestStation;
        }

        //Calculates distances between two addresses using the Haversine Formula
        static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double earthRadius = 6371; // Radius of the Earth in kilometers

            // Convert latitude and longitude from degrees to radians
            double lat1Rad = Math.PI * lat1 / 180;
            double lon1Rad = Math.PI * lon1 / 180;
            double lat2Rad = Math.PI * lat2 / 180;
            double lon2Rad = Math.PI * lon2 / 180;

            // Calculate differences in latitude and longitude
            double latDiff = lat2Rad - lat1Rad;
            double lonDiff = lon2Rad - lon1Rad;

            // Calculate distance using Haversine formula
            double a = Math.Pow(Math.Sin(latDiff / 2), 2) +
                       Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Pow(Math.Sin(lonDiff / 2), 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = earthRadius * c;

            return distance;
        }

        static IEnumerable<GhcndStation> ParseWeatherData(string[] lines)
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
                yield return new GhcndStation
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
