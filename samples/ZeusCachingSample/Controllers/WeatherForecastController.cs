using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZeusCaching;

namespace ZeusCachingSample.Controllers
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



        [HttpGet("basic"), ZeusCache(30)]
        public IEnumerable<WeatherForecast> Get()
        {
            _logger.LogInformation($"Cache missed when calling {Request.Path}");

            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }



        [HttpGet("action-result"), ZeusCache(30)]
        public IActionResult GetBasic()
        {
            _logger.LogInformation($"Cache missed when calling {Request.Path}");

            var rng = new Random();
            var items = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            return Ok(items);
        }


        [WrapResult]
        [HttpGet("embedded"), ZeusCache("WrappedProfile", absoluteExpirationInSeconds: 30)]
        public IEnumerable<WeatherForecast> GetEmbedded()
        {
            _logger.LogInformation($"Cache missed when calling {Request.Path}");

            var rng = new Random();
            var now = DateTime.Now;
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }



        [HttpGet("scalar"), ZeusCache(30)]
        public int GetScalarValue()
        {
            _logger.LogInformation($"Cache missed when calling {Request.Path}");

            var rng = new Random();
            return rng.Next(100);
        }



        [HttpGet("disabled"), ZeusCache("DisabledProfile", absoluteExpirationInSeconds: 30)]
        public async Task<DateTime> GetScalarDisabledValueAsync()
        {
            _logger.LogInformation($"Cache missed when calling {Request.Path}");

            await Task.Delay(1000);
            return DateTime.Now;
        }

    }
}
