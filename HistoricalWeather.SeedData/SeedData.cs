using HistoricalWeather.Domain.Models;
using HistoricalWeather.EF;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Reflection;

namespace HistoricalWeather.SeedData
{
    internal class SeedData
    {
        protected static NoaaWeatherContext context;
        protected static SqlBulkCopy sqlBulkCopy;
        protected static IConfigurationRoot config;

        private static void Main()
        {
            config = new ConfigurationBuilder()
           .AddUserSecrets<SeedData>()
           .Build();
            DbContextOptionsBuilder<NoaaWeatherContext> options = new();

            Console.WriteLine("Beginning Data Seeding for Historical Weather...");

            context = new NoaaWeatherContext(options: options.UseSqlServer(config["ConnectionString"]).Options);

            sqlBulkCopy = new SqlBulkCopy(config["ConnectionString"])
            {
                BulkCopyTimeout = 120
            };

            BulkAddStationRecords();
            BulkAddStationDataTypeRecords();
            BulkAddWeatherRecords();

        }

        protected static void BulkAddStationRecords()
        {
            sqlBulkCopy.DestinationTableName = "Stations";

            IEnumerable<Station> stations = ParseStationData(config["StationDirectory"]);
            int stationCount = context.Stations.Count();

            if (stationCount != stations.Count())
            {
                Console.WriteLine("Adding Stations...");
                context.Database.ExecuteSqlRaw("DELETE FROM Stations");
                context.SaveChanges();

                DataTable stationTable = CreateDataTableCombined(stations);
                sqlBulkCopy.WriteToServer(stationTable);
                Console.WriteLine("Station Data completed.");
            }
        }

        protected static void BulkAddStationDataTypeRecords()
        {
            sqlBulkCopy.DestinationTableName = "StationDataTypes";

            IEnumerable<StationDataType> stations = ParseStationTypeData(config["StationTypeDirectory"]);
            int recordCount = context.StationDataTypes.Count();

            if (recordCount != stations.Count())
            {
                Console.WriteLine("Removing Station Data Types...");
                context.Database.ExecuteSqlRaw("TRUNCATE TABLE StationDataTypes");
                context.SaveChanges();

                Console.WriteLine("Adding Station Data Types...");
                DataTable stationTable = CreateDataTableCombined(stations);
                sqlBulkCopy.WriteToServer(stationTable);
                Console.WriteLine("Station Data Types completed.");
            }
        }

        protected static void BulkAddWeatherRecords()
        {
            //no way to validate all the data for all WeatherRecords.
            sqlBulkCopy.DestinationTableName = "WeatherRecords";
            string? filePath = config["WeatherRecordDirectory"];

            if (string.IsNullOrEmpty(filePath))
                throw new ArgumentNullException("File path cannot be null or empty");

            context.Database.ExecuteSqlRaw("TRUNCATE TABLE WeatherRecords");

            int i = 1;
            int stationCount = Directory.GetFiles(filePath).Length;
            DataTable weatherTable = CreateDataTable<WeatherRecord>();

            foreach (string file in Directory.GetFiles(filePath))
            {
                AddDataTableRecords(weatherTable, ParseWeatherRecordDayFile(file));
                Console.WriteLine($"Scanned {i} out of {stationCount} Stations worth of WeatherRecords");

                sqlBulkCopy.WriteToServer(weatherTable);
                weatherTable.Rows.Clear();
                Console.WriteLine($"Finished saving {i} WeatherRecords");
                i++;
            }
        }

        public static IEnumerable<WeatherRecord> ParseWeatherRecordDayFile(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentNullException(nameof(fileName));

            List<WeatherRecord> weatherRecordDays = [];

            IEnumerable<string> lines = File.ReadLines(fileName);

            foreach (string line in lines)
            {
                for (int i = 0; i < 31; i++)
                {
                    int startIndex = 21 + (i * 8);

                    WeatherRecord day = new()
                    {
                        StationId = line.Substring(0, 11).Trim(),
                        Year = int.Parse(line.Substring(11, 4).Trim()),
                        Month = int.Parse(line.Substring(15, 2).Trim()),
                        Element = line.Substring(17, 4).Trim(),
                        Day = i + 1,
                        Value = int.Parse(line.Substring(startIndex, 5).Trim()),
                        MFlag = line[startIndex + 5],
                        QFlag = line[startIndex + 6],
                        SFlag = line[startIndex + 7]
                    };
                    weatherRecordDays.Add(day);
                }
            }

            return weatherRecordDays;
        }

        public static DataTable CreateDataTableCombined<T>(IEnumerable<T> list)
        {
            DataTable dataTable = CreateDataTable<T>();
            AddDataTableRecords(dataTable, list);
            return dataTable;
        }

        public static void AddDataTableRecords<T>(DataTable dataTable, IEnumerable<T> list)
        {
            Type type = typeof(T);

            //skip virtual properties as they aren't actually columns in the db
            PropertyInfo[] properties = type.GetProperties().Where(p => p.GetMethod != null && !p.GetMethod.IsVirtual).ToArray();

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity) ?? DBNull.Value;
                }

                dataTable.Rows.Add(values);
            }
        }
        public static DataTable CreateDataTable<T>()
        {
            Type type = typeof(T);

            //skip virtual properties as they aren't actually columns in the db
            PropertyInfo[] properties = type.GetProperties().Where(p => p.GetMethod != null && !p.GetMethod.IsVirtual).ToArray();

            DataTable dataTable = new()
            {
                TableName = type.FullName
            };

            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            return dataTable;
        }

        public static IEnumerable<StationDataType> ParseStationTypeData(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name cannot be null or empty", nameof(fileName));

            string[] lines = File.ReadAllLines(fileName);

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

        public static IEnumerable<Station> ParseStationData(string? fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException("File name cannot be null or empty", nameof(fileName));

            string[] lines = File.ReadAllLines(fileName);

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
    }
}
