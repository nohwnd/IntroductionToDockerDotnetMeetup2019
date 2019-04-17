using Microsoft.AspNetCore.Mvc;
using Nohwnd.Meteo.Core;

namespace Nohwnd.Meteo.Api.Controllers
{
    [Route("api/[controller]")]
    public class WeatherController : Controller
    {
        private readonly IWeatherService _service;

        public WeatherController(IWeatherService service)
        {
            _service = service;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_service.GetWeatherEverywhere());
        }

        [HttpGet("{city}")]
        public IActionResult Get(string city)
        {
            if (string.IsNullOrWhiteSpace(city))
                return BadRequest();

            var weather = _service.GetWeatherInCity(city);
            if (weather != null)
                return Ok(weather);

            return NotFound();
        }
    }
}
