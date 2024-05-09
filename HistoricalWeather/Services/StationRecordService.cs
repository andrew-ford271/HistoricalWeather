using HistoricalWeather.Domain.Models;

namespace HistoricalWeather.Services
{
    public class StationRecordService
    {
        public static IEnumerable<WeatherRecordMonth> ParseFile(string[] lines)
        {

            foreach (string line in lines)
            {
                WeatherRecordMonth record = new()
                {
                    StationId = line.Substring(0, 11).Trim(),
                    Year = int.Parse(line.Substring(11, 4).Trim()),
                    Month = int.Parse(line.Substring(15, 2).Trim()),
                    Element = line.Substring(17, 4).Trim(),
                    Days = []
                };

                for (int i = 0; i < 31; i++)
                {
                    int startIndex = 21 + (i * 8);
                    WeatherRecordDay day = new()
                    {
                        Value = int.Parse(line.Substring(startIndex, 5).Trim()),
                        MFlag = line[startIndex + 5],
                        QFlag = line[startIndex + 6],
                        SFlag = line[startIndex + 7]
                    };
                    record.Days.Add(day);
                }

                yield return record;
            }
        }
    }
}
