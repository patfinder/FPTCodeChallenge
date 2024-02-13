using WeatherDTO;

namespace WeatherServiceInterface
{
    public interface IWeatherForecast
    {
        public Task<WeatherForecastDTO> GetCurrentWeather(string location);
    }
}
