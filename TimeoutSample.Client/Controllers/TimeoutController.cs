using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace TimeoutSample.Client.Controllers
{
    public class TimeoutController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public TimeoutController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(3);

            try
            {
                HttpResponseMessage result = 
                    await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://localhost:6001/api/WeatherForecast"));
                var content = await result.Content.ReadAsStringAsync();

                return new JsonResult(new { result = JsonConvert.DeserializeObject<List<WeatherForecast>>(content) });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
