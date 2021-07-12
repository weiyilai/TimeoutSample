using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TimeoutSample.Job
{
    public interface IMyService
    {
        Task<bool> ConnectAsync();
    }

    public class MyService : IMyService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public MyService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> ConnectAsync()
        {
            var client = _httpClientFactory.CreateClient();
            client.Timeout = TimeSpan.FromSeconds(3);

            var result = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "https://localhost:6001/api/WeatherForecast"));
            var content = await result.Content.ReadAsStringAsync();

            return false;
        }
    }
}
