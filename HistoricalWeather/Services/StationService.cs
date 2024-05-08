using HistoricalWeather.Models;

namespace HistoricalWeather.Services
{
    public class StationService
    {
        public StationService()
        {
        }

        public static GhcndStation FindClosestStation(IEnumerable<GhcndStation> ghcndStations, double targetLatitude, double targetLongitude)
        {
            if (!ghcndStations.Any())
            {
                throw new InvalidOperationException("No stations available.");
            }

            GhcndStation closestStation = ghcndStations.First();
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

        public static IEnumerable<GhcndStation> ParseStationData(string[] lines)
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

        public static IEnumerable<GhcndIndex> ParseStationIndexData(string[] lines)
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
                yield return new GhcndIndex
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

        //Calculates distances between two addresses using the Haversine Formula
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            // Validate latitude and longitude
            if (!IsValidLatitude(lat1) || !IsValidLongitude(lon1) || !IsValidLatitude(lat2) || !IsValidLongitude(lon2))
            {
                throw new ArgumentException("Invalid latitude or longitude.");
            }

            const double EARTH_RADIUS = 6371; // Radius of the Earth in kilometers

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
            double distance = EARTH_RADIUS * c;

            return distance;
        }
        private static bool IsValidLatitude(double latitude)
        {
            return latitude >= -90 && latitude <= 90;
        }

        // Function to validate longitude
        private static bool IsValidLongitude(double longitude)
        {
            return longitude >= -180 && longitude <= 180;
        }
    }
}
