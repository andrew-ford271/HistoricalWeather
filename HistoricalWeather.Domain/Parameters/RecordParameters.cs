namespace HistoricalWeather.Domain.Parameters
{
    public class RecordParameters : BaseParameters
    {
        public int? Month { get; set; }

        public int? Year { get; set; }

        public int? Day { get; set; }

        public string? ObservationType { get; set; }
    }
}
