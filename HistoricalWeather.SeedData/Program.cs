using HistoricalWeather.Domain.Models;
using HistoricalWeather.EF.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace HistoricalWeather.SeedData
{
    internal class Program
    {
        protected static NoaaWeatherContext context;
        protected static SqlBulkCopy sqlBulkCopy;

        static void Main()
        {
            Console.WriteLine("Beginning Data Seeding for Historical Weather...");
            context = new NoaaWeatherContext();
            sqlBulkCopy.BulkCopyTimeout = 120;
            //sqlBulkCopy.BatchSize = 24270262;

            BulkAddStationRecords();
            BulkAddStationDataTypeRecords();
            BulkAddWeatherRecords();

        }

        protected static void BulkAddStationRecords()
        {
            sqlBulkCopy.DestinationTableName = "Stations";

            IEnumerable<Station> stations = ParseStationData("../../../ghcnd-files/ghcnd-stations.txt");
            int stationCount = context.Stations.Count();

            if (stationCount != stations.Count())
            {
                Console.WriteLine("Adding Stations...");
                context.Database.ExecuteSqlRaw("DELETE FROM Stations");
                context.SaveChanges();

                DataTable stationTable = CreateDataTable(stations);
                sqlBulkCopy.WriteToServer(stationTable);
                Console.WriteLine("Station Data completed.");
            }
        }

        protected static void BulkAddStationDataTypeRecords()
        {
            sqlBulkCopy.DestinationTableName = "StationDataTypes";

            IEnumerable<StationDataType> stations = ParseStationIndexData("../../../ghcnd-files/ghcnd-inventory.txt");
            int recordCount = context.StationDataTypes.Count();

            if (recordCount != stations.Count())
            {
                Console.WriteLine("Removing Station Data Types...");
                context.Database.ExecuteSqlRaw("TRUNCATE TABLE StationDataTypes");
                context.SaveChanges();

                Console.WriteLine("Adding Station Data Types...");
                DataTable stationTable = CreateDataTable(stations);
                sqlBulkCopy.WriteToServer(stationTable);
                Console.WriteLine("Station Data Typecompleted.");
            }
        }

        protected static void BulkAddWeatherRecords()
        {
            //no way to validate all the data for all WeatherRecords.
            sqlBulkCopy.DestinationTableName = "WeatherRecordDays";
            var filePath = $"C:\\Users\\12254\\Downloads\\ghcnd_all\\";
            context.Database.ExecuteSqlRaw("TRUNCATE TABLE WeatherRecordDays");

            int i = 1;
            var stationCount = Directory.GetFiles(filePath).Length;
            List<WeatherRecordDay> weatherRecords = [];
            DataTable dbTable;

            foreach (string file in Directory.GetFiles(filePath))
            {
                weatherRecords.AddRange(ParseWeatherRecordDayFile(file));

                if (i % 100 == 0)
                    Console.WriteLine($"Scanned {i} out of {stationCount} Stations worth of WeatherRecords");

                if (i % 250 == 0)
                {
                    dbTable = CreateDataTable(weatherRecords);
                    weatherRecords.Clear();
                    sqlBulkCopy.WriteToServer(dbTable);
                    dbTable.Clear();
                    Console.WriteLine($"Finished saving {i} WeatherRecords");
                }

                i++;
            }

            //Get last set of records
            dbTable = CreateDataTable(weatherRecords);
            sqlBulkCopy.WriteToServer(dbTable);
            weatherRecords.Clear();
            Console.WriteLine($"Finished saving {i} WeatherRecords");

            context.Database.ExecuteSqlRaw("TRUNCATE TABLE WeatherRecordDays");
        }

        public static IEnumerable<WeatherRecordDay> ParseWeatherRecordDayFile(string file)
        {

            List<WeatherRecordDay> weatherRecordDays = [];

            var lines = File.ReadLines(file);

            foreach (var line in lines)
            {
                for (int i = 0; i < 31; i++)
                {
                    int startIndex = 21 + (i * 8);

                    WeatherRecordDay day = new()
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

        public static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);

            //skip virtual properties as they aren't actually columns in the db
            var properties = type.GetProperties().Where(p => p.GetMethod != null && !p.GetMethod.IsVirtual).ToArray();

            DataTable dataTable = new DataTable();
            dataTable.TableName = typeof(T).FullName;
            foreach (PropertyInfo info in properties)
            {

                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }

        public static IEnumerable<StationDataType> ParseStationIndexData(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

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

        public static IEnumerable<Station> ParseStationData(string fileName)
        {
            var lines = File.ReadAllLines(fileName);

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

        //public static IEnumerable<WeatherRecordMonth> ParseWeatherRecordFile(string file)
        //{
        //    var lines = File.ReadLines(file);

        //    foreach (string line in lines)
        //    {
        //        WeatherRecordMonth record = new()
        //        {
        //            StationId = line.Substring(0, 11).Trim(),
        //            Year = int.Parse(line.Substring(11, 4).Trim()),
        //            Month = int.Parse(line.Substring(15, 2).Trim()),
        //            Element = line.Substring(17, 4).Trim(),
        //        };

        //        yield return record;
        //    }
        //}
    }
}
