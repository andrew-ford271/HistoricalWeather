using System.ComponentModel.DataAnnotations.Schema;

namespace HistoricalWeather.Domain.Models
{
    public class WeatherRecordMonth
    {
        public int Id { get; set; }

        [Column(TypeName = "char(11)")]
        [ForeignKey("Station")]
        public string StationId { get; set; }

        [Column(TypeName = "int")]
        public int Year { get; set; }

        [Column(TypeName = "int")]
        public int Month { get; set; }

        [Column(TypeName = "char(4)")]
        public string Element { get; set; }

        public virtual Station Station { get; set; }
        
        public virtual ICollection<WeatherRecordDay> Days { get; set; }
    }
}