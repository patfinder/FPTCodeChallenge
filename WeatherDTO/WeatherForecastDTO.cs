using System.Text.Json.Serialization;

namespace WeatherDTO
{
    public class WeatherForecastDTO
    {
        [JsonPropertyName("current")]
        public CurrentForcastDTO Current { get; set; }
    }
}
