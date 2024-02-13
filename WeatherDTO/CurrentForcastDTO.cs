using System.Text.Json.Serialization;

namespace WeatherDTO
{
    public class CurrentForcastDTO
    {
        //[JsonPropertyName("observation_time")]
        //public string ObservationTime { get; set;}
        //public string Temperature { get; set;}
        //[JsonPropertyName("weather_code")]
        //public string WeatherCode { get; set;}
        //[JsonPropertyName("weather_icons")]
        //public string WeatherIcons { get; set;}
        //[JsonPropertyName("weather_descriptions")]
        //public string WeatherDescriptions { get; set;}
        [JsonPropertyName("Wind_speed")]
        public int WindSpeed { get; set; }
        //[JsonPropertyName("wind_degree")]
        //public string WindDegree { get; set;}
        //[JsonPropertyName("wind_dir")]
        //public string WindDir { get; set;}
        //public string Pressure { get; set;}
        public int Precip { get; set; }
        //public string Humidity { get; set;}
        //public string Cloudcover { get; set;}
        //public string Feelslike { get; set;}
        [JsonPropertyName("uv_index")]
        public int UvIndex { get; set; }
        //public string Visibility { get; set;}
        //[JsonPropertyName("is_day")]
        //public string IsDay { get; set;}
    }
}
