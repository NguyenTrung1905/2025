using _2025.Services.AuthAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace _2025.Services.AuthAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHelloService _helloService;
        private readonly AppSettings _appSettings;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHelloService helloService, AppSettings appSettings)
        {
            _logger = logger;
            _helloService = helloService;
            _appSettings = appSettings;
        }

        //[HttpGet("get-config")]
        //public List<int> GetConfig()
        //{
        //    return _appSettings.SampleConfig;
        //}

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogTrace("LogTrace");
            _logger.LogDebug("LogDebug");
            _logger.LogInformation("LogInformation");
            _logger.LogWarning("LogWarning");
            _logger.LogError("LogError");
            _logger.LogCritical("LogCritical");

            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("say-Hello")]
        public string SayHello([FromQuery] string  name)
        {
            return _helloService.sayHello(name);
        }
    }
}
