using IAdviseService;
using WeatherDTO;
using WeatherServiceInterface;

namespace AdviseService
{
    public class AdviceService : IAdviceService
    {
        private IWeatherForecast WeatherForecast;
        public AdviceService(IWeatherForecast weatherForecast)
        {
            WeatherForecast = weatherForecast;
        }
        public async Task<AdviceResultDTO> GetAdvice(string location)
        {
            var weather = await WeatherForecast.GetCurrentWeather(location);

            var result = new AdviceResultDTO
            {
                Precip = weather.Current.Precip,
                GoOutside = weather.Current.Precip == 0,
                UvIndex = weather.Current.UvIndex,
                WearSunscreen = weather.Current.UvIndex > 3,
                WindSpeed = weather.Current.WindSpeed,
                FlyKite = weather.Current.WindSpeed > 15
            };

            return result;
        }
    }
}
