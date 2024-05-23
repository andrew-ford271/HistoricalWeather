namespace HistoricalWeather.Api.Services
{
    public static class DistanceHelper
    {
        private const double EARTH_RADIUS = 6371;

        ///<summary>Calculates distances between two addresses using the Haversine Formula </summary>
        public static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            if (!IsValidCoordinate(lat1, lon1))
                throw new ArgumentException("Must contain a valid latitude and longitude");

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
                       (Math.Cos(lat1Rad) * Math.Cos(lat2Rad) *
                       Math.Pow(Math.Sin(lonDiff / 2), 2));
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = EARTH_RADIUS * c;

            return distance;
        }
        public static bool IsValidCoordinate(double? latitude, double? longitude)
        {
            return (latitude is >= (-90) and <= 90) && (longitude is >= (-180) and <= 180);
        }
    }
}