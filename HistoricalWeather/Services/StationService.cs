using HistoricalWeather.Domain.Models;
using HistoricalWeather.Domain.Parameters;
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

        public IEnumerable<Station> GetAllStations(StationParameters parameters)
        {
            IEnumerable<Station> stations = context.Stations.AsEnumerable();
            if (parameters.Latitude != null && parameters.Longitude != null)
                stations = stations.OrderBy(station => DistanceHelper.CalculateDistance(parameters.Latitude.Value, parameters.Longitude.Value, station.Latitude, station.Longitude));

            if (parameters.StationName != null)
                stations = stations.Where(x => x.StationName.Contains(parameters.StationName, StringComparison.OrdinalIgnoreCase));

            if (parameters.State != null)
                stations = stations.Where(x => x.State != null && x.State.Contains(parameters.State, StringComparison.OrdinalIgnoreCase));
            return stations.Skip(parameters.Offset ?? 0).Take(parameters.Limit ?? 100);
        }

        public IEnumerable<StationObservationType> GetStationObservationTypes(string stationId)
        {
            return context.StationObservationTypes.Where(x => x.StationId == stationId);
        }

        public Station? GetStation(string stationId)
        {
            return context.Stations.Where(x => x.Id == stationId).FirstOrDefault();
        }
    }
}
