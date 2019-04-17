using System.Collections.Generic;

namespace Nohwnd.Meteo.Core
{
    public interface IWeatherRepository
    {
        IReadOnlyCollection<Weather> GetAll();
        Weather GetOne(string city);
    }
}