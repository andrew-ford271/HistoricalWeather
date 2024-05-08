using System;

namespace HistoricalWeather.Models
{
    public class WeatherRecord
    {
        public string ID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public string Element { get; set; }

        public List<WeatherRecordDay> Days {  get; set; }
    }
}