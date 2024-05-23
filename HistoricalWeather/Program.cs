using HistoricalWeather.Api.Services;
using HistoricalWeather.Domain.Models;
using HistoricalWeather.Domain.Parameters;
using HistoricalWeather.EF;
using Microsoft.EntityFrameworkCore;

namespace HistoricalWeather.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();
            builder.Services.AddScoped<StationService>();
            builder.Services.AddScoped<RecordService>();
            builder.Services.AddDbContext<NoaaWeatherContext>(options => options.UseSqlServer(builder.Configuration["ConnectionString"]));
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapGet("/ClosestStation", (double latitude, double longitude, NoaaWeatherContext noaaWeatherContext, StationService stationService) =>
            {
                return stationService.FindClosestStation(latitude, longitude);
            })
            .WithOpenApi();

            app.MapGet("/Stations", (NoaaWeatherContext noaaWeatherContext, StationService stationService, int limit = 10, int offset = 0) =>
            {
                return stationService.GetAllStations(limit, offset);
            })
            .WithOpenApi();

            app.MapGet("/Stations/{stationId}", (string stationId, NoaaWeatherContext noaaWeatherContext, StationService stationService, int limit = 10, int offset = 0) =>
            {
                Station? station = stationService.GetStation(stationId);
                return station == null ? Results.NotFound() : Results.Ok(station);
            })
            .WithOpenApi();

            app.MapGet("/Stations/{stationId}/WeatherRecords", (string stationId, NoaaWeatherContext noaaWeatherContext, RecordService recordService, [AsParameters] RecordParameters recordParameters) =>
            {
                return recordService.GetWeatherRecords(stationId, recordParameters);
            })
            .WithName("Records")
            .WithOpenApi();
            app.Run();
        }
    }
}
