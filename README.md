# NetBenchmark
http performance benchmark tools
## http
``` csharp
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

            var runer = NetBenchmark.Benchmark.Http("test", clientFactory, 1, 200, async http =>
            {
                var response = await http.PostAsync("/v1/api/Quote/GetSymbol", new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "id", "1" }
                }));
            });

            runer.Run();
            Console.ReadKey();
        }
    }
```
## http results
![image](https://raw.githubusercontent.com/zzq424/Store/master/images/NetBenchmark/NetBenchmark_http_test.png)