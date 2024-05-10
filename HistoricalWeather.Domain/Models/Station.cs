﻿using System.ComponentModel.DataAnnotations.Schema;

namespace HistoricalWeather.Domain.Models
{
    public class Station
    {
        [Column(TypeName = "char(11)")]
        public required string Id { get; set; }

        [Column(TypeName = "decimal(6,4)")]
        public required double Latitude { get; set; }

        [Column(TypeName = "decimal(7,4)")]
        public required double Longitude { get; set; }

        [Column(TypeName = "decimal(5,1)")]
        public required double Elevation { get; set; }

        [Column(TypeName = "char(2)")]
        public string? State { get; set; }

        [Column(TypeName = "char(32)")]
        public required string StationName { get; set; }
    }
}
