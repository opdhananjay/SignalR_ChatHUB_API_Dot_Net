using Microsoft.AspNetCore.Mvc;

namespace TC.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            TakeParam(1, 3, 4, 5, 6, 7, 8, 8);
            int ss = calculateSums(10, 20, 30);
            Console.WriteLine(ss);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        
        public string TakeParam(params int[] num)
        {
            return $"{num}";
        }

        // Use Case - 
        public int calculateSums(params int[] prices)
        {
            int sum = prices.Sum();
            return sum;
        }

    }


 

}
