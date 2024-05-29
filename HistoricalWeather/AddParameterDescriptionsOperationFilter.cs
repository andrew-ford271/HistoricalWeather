using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace HistoricalWeather.Api
{
    public class AddParameterDescriptionsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.AddOpenApiParameterDescriptions();
        }
    }
}