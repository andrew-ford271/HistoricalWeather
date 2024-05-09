using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HistoricalWeather.Domain.Models
{
    public class WeatherRecordDay
    {
        public int Id { get; set; }

        [Column(TypeName = "char(4)")]
        public int Value { get; set; }

        [Column(TypeName = "char(1)")]
        public char MFlag { get; set; }

        [Column(TypeName = "char(1)")]
        public char QFlag { get; set; }

        [Column(TypeName = "char(1)")]
        public char SFlag { get; set; }
    }
}