using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetBenchmark
{
    public class HttpTester : ITester, IHttpHandler
    {

        public HttpTester(HttpClient client)
        {
            _httpClient = client;
        }

        private HttpClient _httpClient;

        public Func<IHttpHandler, Task> Handler { get; set; }

        public Runner Runner { get; set; }

        public async Task Execute()
        {
            await Handler(this);
        }

        public async Task<HttpContent> GetAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);
            return response.Content;
        }

        public async Task<HttpContent> PostAsync(string url, HttpContent content)
        {
            var response = await _httpClient.PostAsync(url, content);
            return response.Content;
        }
    }
}
