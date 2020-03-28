using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetBenchmark
{
    public class Benchmark
    {
        public static Runner Http(string name, IHttpClientFactory clientFactory, int connections, int interval, Func<IHttpHandler, Task<bool>> handler)
        {
            Runner runer = new Runner() { Interval = interval };
            runer.Name = $"HTTP {name} [Connections:{connections:###,###,###}]";
            for (int i = 0; i < connections; i++)
            {
                HttpTester tester = new HttpTester(clientFactory.CreateClient(name));
                tester.Handler = handler;
                tester.Runner = runer;
                runer.Testers.Add(tester);
            }
            return runer;
        }

        public static Runner Http(IHttpClientFactory clientFactory, int connections, int interval, Func<IHttpHandler, Task<bool>> handler)
        {
            Runner runer = new Runner() { Interval = interval };
            runer.Name = $"HTTP [Connections:{connections:###,###,###}]";
            for (int i = 0; i < connections; i++)
            {
                HttpTester tester = new HttpTester(clientFactory.CreateClient());
                tester.Handler = handler;
                tester.Runner = runer;
                runer.Testers.Add(tester);
            }
            return runer;
        }
    }
}
