using HistoricalWeather.Domain.Models;
using HistoricalWeather.EF;

namespace HistoricalWeather.Api.Services
{
    public class StationService(NoaaWeatherContext context)
    {
        protected readonly NoaaWeatherContext context = context;

        public Station FindClosestStation(double targetLatitude, double targetLongitude)
        {
            if (!DistanceHelper.IsValidCoordinate(targetLatitude, targetLongitude))
                throw new ArgumentException("Invalid latitude or longitude.");

            Station closestStation = context.Stations.First();
            double minDistance = double.MaxValue;

            foreach (Station station in context.Stations)
            {
                double distance = DistanceHelper.CalculateDistance(targetLatitude, targetLongitude, station.Latitude, station.Longitude);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestStation = station;
                }
            }

            return closestStation;
        }

        public IEnumerable<Station> GetAllStations(int limit, int offset)
        {
            return context.Stations.Skip(offset).Take(limit);
        }

        public Station? GetStation(string stationId)
        {
            return context.Stations.Where(x => x.Id == stationId).FirstOrDefault();
        }
    }
}
