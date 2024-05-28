using Microsoft.OpenApi.Models;

namespace HistoricalWeather.Api
{
    public static class OpenApiExtensions
    {
        private const string limit_description = "Represents how many elements to take from the list";
        private const string offset_description = "Represents how many elements from the start of the list are skipped.";
        private const string observation_type_description = "Represents the type of weather to filter on.";
        private const string station_id_description = "Represents the 11 character NOAA station name";

        public static void AddOpenApiParameterDescriptions(this OpenApiOperation operation)
        {
            if (operation.Parameters.Any(x => x.Name == "Offset"))
                operation.Parameters.First(x => x.Name == "Offset").Description = offset_description;

            if (operation.Parameters.Any(x => x.Name == "Limit"))
                operation.Parameters.First(x => x.Name == "Limit").Description = limit_description;

            if (operation.Parameters.Any(x => x.Name == "ObservationType"))
                operation.Parameters.First(x => x.Name == "ObservationType").Description = observation_type_description;

            if (operation.Parameters.Any(x => x.Name == "stationId"))
                operation.Parameters.First(x => x.Name == "stationId").Description = station_id_description;
        }
    }
}