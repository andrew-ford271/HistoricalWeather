using System.ComponentModel.DataAnnotations.Schema;

namespace HistoricalWeather.Domain.Models
{
    public class StationDataType
    {

        public int Id { get; set; }

        [ForeignKey("Station")]
        [Column(TypeName = "char(11)")]
        public required string StationId { get; set; }

        [Column(TypeName = "decimal(2,4)")]
        public required double Latitude { get; set; }

        [Column(TypeName = "decimal(3,4)")]
        public required double Longitude { get; set; }

        [Column(TypeName = "char(4)")]
        public required string Value { get; set; }

        public required int StartDate { get; set; }

        public required int EndDate { get; set; }

        public virtual Station Station { get; set; }
    }
}
