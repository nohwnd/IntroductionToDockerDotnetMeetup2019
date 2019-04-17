using System.Collections.Generic;

namespace Nohwnd.Meteo.Core
{
    public interface IWeatherService
    {
        Weather GetWeatherInCity(string city);
        IReadOnlyCollection<Weather> GetWeatherEverywhere();
    }
}