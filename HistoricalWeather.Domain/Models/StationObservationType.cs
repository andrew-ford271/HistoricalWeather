using System.ComponentModel.DataAnnotations.Schema;

namespace HistoricalWeather.Domain.Models
{
    public class StationObservationType
    {
        public int Id { get; set; }

        [ForeignKey("Station")]
        [Column(TypeName = "char(11)")]
        public required string StationId { get; set; }

        [Column(TypeName = "char(4)")]
        public required string ObservationType { get; set; }

        public required int StartDate { get; set; }

        public required int EndDate { get; set; }

        public virtual Station Station { get; set; }
    }
}
