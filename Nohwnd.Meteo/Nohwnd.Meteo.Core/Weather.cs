using System.Collections;

namespace Nohwnd.Meteo.Core
{
    public class Weather
    {
        public string Id { get; set; }
        public string City { get; set; }
        public WeatherType WeatherType { get; set; }
    }
}