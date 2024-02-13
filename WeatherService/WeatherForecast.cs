using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Http.Json;
using WeatherDTO;
using WeatherServiceInterface;

namespace WeatherService
{
    public class WeatherForecast : IWeatherForecast
    {
        private readonly IConfiguration Configuration;

        public WeatherForecast(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public async Task<WeatherForecastDTO> GetCurrentWeather(string location)
        {
            var httpClient = new HttpClient();

            // http://api.weatherstack.com/
            var baseUrl = Configuration.GetSection("WeatherAPI")["BaseURL"];
            var accessKey = Configuration["WEATHER_API_KEY"];
            var uri = $"{baseUrl}current?access_key={accessKey}&query={location}";
            var weatherForecast = await httpClient.GetFromJsonAsync<WeatherForecastDTO>(uri);

            return weatherForecast;
        }
    }
}
