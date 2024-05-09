using HistoricalWeather.Domain.Models;
using HistoricalWeather.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace HistoricalWeather.SeedData
{
    internal class Program
    {
        static async Task Main()
        {
            Console.WriteLine("Beginning Data Seeding for Historical Weather...");

            using var db = new NoaaWeatherContext();

            var stationLines = await File.ReadAllLinesAsync("../../../ghcnd-files/ghcnd-stations.txt");
            IEnumerable<Station> stations = ParseStationData(stationLines);
            
            if (db.Stations.Count() != stations.Count())
            {
                Console.WriteLine("Adding Stations...");
                db.Database.ExecuteSqlRaw("DELETE FROM Stations");
                db.Stations.AddRange(stations);
                db.SaveChanges();
                Console.WriteLine("Station Data completed.");
            }

            var stationInventorylines = await File.ReadAllLinesAsync("../../../ghcnd-files/ghcnd-inventory.txt");
            IEnumerable<StationDataType> stationDataTypes = ParseStationIndexData(stationInventorylines);
            
            if (db.StationDataTypes.Count() != stationDataTypes.Count())
            {
                Console.WriteLine(" Adding StationDataTypes....");
                db.Database.ExecuteSqlRaw("DELETE FROM StationDataType");
                db.StationDataTypes.AddRange(stationDataTypes);
                db.SaveChanges();
                Console.WriteLine("Station DataTypes completed.");
            }

            //no way to validate all the data for All WeatherRecords.
            var filePath = $"\\Downloads\\ghcnd_all\\";
            db.Database.ExecuteSqlRaw("DELETE FROM WeatherRecordDays");
            db.Database.ExecuteSqlRaw("DELETE FROM WeatherRecordMonths");

            int i = 0;
            foreach (string file in Directory.GetFiles(filePath))
            {
                //if (i % 100 == 0)
                Console.WriteLine($"Completed adding {i} Stations worth of WeatherRecords");

                var lines = await File.ReadAllLinesAsync(file);
                var weatherRecords = ParseWeatherRecordFile(lines);
                db.WeatherRecordMonths.AddRange(weatherRecords);
                //db.SaveChanges();
                i++;
                if (i % 100 == 0)
                {
                    Console.WriteLine($"Saved {i} stations worth of WeatherRecords");
                    db.SaveChanges();
                }

            }
        }

        public static IEnumerable<StationDataType> ParseStationIndexData(string[] lines)
        {
            foreach (string line in lines)
            {
                // Parsing based on number of characters
                string stationId = line.Substring(0, 11).Trim();
                double latitude = double.Parse(line.Substring(12, 8).Trim());
                double longitude = double.Parse(line.Substring(21, 9).Trim());
                string value = line.Substring(31, 4).Trim();
                int startDate = int.Parse(line.Substring(36, 4).Trim());
                int endDate = int.Parse(line.Substring(41, 4).Trim());

                // Creating and returning WeatherData object
                yield return new StationDataType
                {
                    StationId = stationId,
                    Latitude = latitude,
                    Longitude = longitude,
                    Value = value,
                    StartDate = startDate,
                    EndDate = endDate
                };
            }
        }
        public static IEnumerable<Station> ParseStationData(string[] lines)
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
                yield return new Station
                {
                    Id = stationId,
                    Latitude = latitude,
                    Longitude = longitude,
                    Elevation = elevation,
                    State = state,
                    StationName = stationName
                };
            }
        }
        public static IEnumerable<WeatherRecordMonth> ParseWeatherRecordFile(string[] lines)
        {

            foreach (string line in lines)
            {
                WeatherRecordMonth record = new()
                {
                    StationId = line.Substring(0, 11).Trim(),
                    Year = int.Parse(line.Substring(11, 4).Trim()),
                    Month = int.Parse(line.Substring(15, 2).Trim()),
                    Element = line.Substring(17, 4).Trim(),
                    Days = []
                };

                for (int i = 0; i < 31; i++)
                {
                    int startIndex = 21 + (i * 8);
                    WeatherRecordDay day = new()
                    {
                        Value = int.Parse(line.Substring(startIndex, 5).Trim()),
                        MFlag = line[startIndex + 5],
                        QFlag = line[startIndex + 6],
                        SFlag = line[startIndex + 7]
                    };
                    record.Days.Add(day);
                }

                yield return record;
            }
        }
    }
}
