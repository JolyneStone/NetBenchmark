using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Simples
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddHttpClient("test", c =>
            {
                c.BaseAddress = new Uri("http://localhost:19876");
            });

            var service = serviceCollection.BuildServiceProvider();
            var clientFactory = service.GetService<IHttpClientFactory>();

            var runer = NetBenchmark.Benchmark.Http("test", clientFactory, 50, 200, async http =>
            {
                var response = await http.PostAsync("/v1/api/Quote/GetSymbol", new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "id", "1" }
                }));
                return true;
            });

            runer.Run();
            Console.ReadKey();
        }
    }
}
