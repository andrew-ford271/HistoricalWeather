namespace HistoricalWeather.Domain.Parameters
{
    public class StationParameters : BaseParameters
    {
        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public string? StationName { get; set; }

        public string? State { get; set; }

        public string? ObservationType { get; set; }
    }
}
