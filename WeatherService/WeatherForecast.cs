using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Registry;
using Polly.Timeout;
using System;
using System.Net;
using System.Net.Http.Json;
using WeatherDTO;
using WeatherServiceInterface;

namespace WeatherService
{
    public class WeatherForecast : IWeatherForecast
    {
        private readonly ILogger<WeatherForecast> Logger;
        private readonly IServiceProvider Services;
        private readonly IConfiguration Configuration;

        public WeatherForecast(IServiceProvider services, IConfiguration configuration)
        {
            Services = services;
            Configuration = configuration;
            Logger = services.GetRequiredService<ILogger<WeatherForecast>>();
        }

        public async Task<WeatherForecastDTO> GetCurrentWeather(string location)
        {
            return await GetWithRetry(location);
        }

        private async Task<WeatherForecastDTO> GetWithRetry(string location)
        {
            // Resolve the resilience pipeline provider for string-based keys
            ResiliencePipelineProvider<string> pipelineProvider = Services.GetRequiredService<ResiliencePipelineProvider<string>>();
            ResiliencePipeline pipeline = pipelineProvider.GetPipeline("weather-pipeline");

            // Acquire a ResilienceContext from the pool
            ResilienceContext context = ResilienceContextPool.Shared.Get();

            // Should call LogInformation or LogDebug
            // Use LogWarning here just for ===== DEMONSTRATION PURPOSE =====
            Logger.LogWarning($"\n\n=============================================");
            Logger.LogWarning($"Getting weather info for location: {location}\n\n");

            // Execute the pipeline
            // Notice in console output that telemetry is automatically enabled
            var outcome = await pipeline.ExecuteOutcomeAsync<WeatherForecastDTO, string>(async (context, state) =>
            {
                var httpClient = new HttpClient();

                // http://api.weatherstack.com/
                var baseUrl = Configuration.GetSection("WeatherAPI")["BaseURL"];
                var accessKey = Configuration["WEATHER_API_KEY"];
                var uri = $"{baseUrl}current?access_key={accessKey}&query={state}";

                var weatherForecast = await httpClient.GetFromJsonAsync<WeatherForecastDTO>(uri, context.CancellationToken);
                return Outcome.FromResult(weatherForecast);
            }, context, location);

            // Return the acquired ResilienceContext to the pool
            ResilienceContextPool.Shared.Return(context);

            if (outcome.Exception != null)
            {
                Logger.LogError($"Weather API exception : {outcome.Exception.Message}");

                throw outcome.Exception;
            }
            else
            {
                if (outcome.Result == null)
                    throw new Exception("Unexpected error: weather result is empty.");

                return outcome.Result;
            }
        }
    }
}
