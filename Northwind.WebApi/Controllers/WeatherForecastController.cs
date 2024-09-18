using Microsoft.AspNetCore.Mvc;

namespace Northwind.WebApi.Controllers
{
    [ApiController] //enable REST specific behavior (Automatic 400 response for invalid models, etc)
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        // GET /weatherforecast
        [HttpGet(Name = "GetWeatherForecastFiveDays")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Get(days: 5); //5 days forecast
        }

        // GET /weatherforecast/7
        [HttpGet(template: "{days:int}", Name = "GetWeatherForecast")] //{days:int} constrains the days parameter to int
        public IEnumerable<WeatherForecast> Get(int days)
        {
            return Enumerable.Range(1, days).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
