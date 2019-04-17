using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Nohwnd.Meteo.Core;

namespace Nohwnd.Meteo.Data.Mongo
{
    public class WeatherRepository : IWeatherRepository
    {
        private readonly IMongoCollection<Weather> _collection;

        public WeatherRepository(IMongoCollection<Weather> collection)
        {
            _collection = collection;
        }

        public IReadOnlyCollection<Weather> GetAll()
        {
            return IAsyncCursorSourceExtensions.ToList(_collection.AsQueryable()).AsReadOnly();
        }

        public Weather GetOne(string city)
        {
            var c = city.ToLowerInvariant();
            return _collection.AsQueryable().SingleOrDefault(w => w.City.ToLowerInvariant() == c);
        }
    }
}