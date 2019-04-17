using System.Collections.Generic;

namespace Nohwnd.Meteo.Core
{
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _repository;

        public WeatherService(IWeatherRepository repository)
        {
            _repository = repository;
        }

        public Weather GetWeatherInCity(string city)
        {
            return _repository.GetOne(city);
        }

        public IReadOnlyCollection<Weather> GetWeatherEverywhere()
        {
            return _repository.GetAll();
        }
    }
}