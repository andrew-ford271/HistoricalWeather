namespace HistoricalWeather.Models
{
    public class GhcndStation
    {
        public required string StationId { get; set; }

        public required double Latitude { get; set; }

        public required double Longitude { get; set; }

        public required double Elevation { get; set; }

        public string? State { get; set; }

        public required string StationName { get; set; }
    }
}
