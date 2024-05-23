using HistoricalWeather.Domain.Models;
using HistoricalWeather.Domain.Parameters;
using HistoricalWeather.EF;

namespace HistoricalWeather.Api.Services
{
    public class RecordService(NoaaWeatherContext context)
    {
        protected readonly NoaaWeatherContext context = context;

        public IEnumerable<WeatherRecord> GetWeatherRecords(string stationId, RecordParameters recordParameters)
        {
            IQueryable<WeatherRecord> records = context.WeatherRecords.Where(x => x.StationId == stationId);

            if (recordParameters.Year != null)
                records = records.Where(x => x.Year == recordParameters.Year);

            if (recordParameters.Month != null)
                records = records.Where(x => x.Month == recordParameters.Month);

            if (recordParameters.Day != null)
                records = records.Where(x => x.Day == recordParameters.Day);

            if (recordParameters.Element != null)
                records = records.Where(x => x.Element == recordParameters.Element);

            return records.Skip(recordParameters.Offset ?? 0).Take(recordParameters.Limit ?? 1000);
        }
    }
}
