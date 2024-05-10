using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HistoricalWeather.Domain.Models
{
    public class WeatherRecordDay
    {
        [Key]
        public long Id { get; set; }

        [Column(TypeName = "char(11)")]
        [ForeignKey("Station")]
        public string StationId { get; set; }

        [Column(TypeName = "int")]
        public int Year { get; set; }

        [Column(TypeName = "int")]
        public int Month { get; set; }

        public int Day { get; set; }

        [Column(TypeName = "char(4)")]
        public string Element { get; set; }

        public int Value { get; set; }

        [Column(TypeName = "nchar(1)")]
        public char MFlag { get; set; }

        [Column(TypeName = "nchar(1)")]
        public char QFlag { get; set; }

        [Column(TypeName = "nchar(1)")]
        public char SFlag { get; set; }

        //public virtual WeatherRecordMonth WeatherRecordMonth { get; set; }
        public virtual Station Station { get; set; }
    }
}