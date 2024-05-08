namespace HistoricalWeather.Models
{
    public class GhcndIndex
    {
        public required string StationId { get; set; }

        public required double Latitude { get; set; }

        public required double Longitude { get; set; }

        public required string Value { get; set; }

        public required int StartDate { get; set; }

        public required int EndDate { get; set; }
    }
}
