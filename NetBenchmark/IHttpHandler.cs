using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NetBenchmark
{
    public interface IHttpHandler
    {
        Task<HttpContent> GetAsync(string url);

        Task<HttpContent> PostAsync(string url, HttpContent httpContent);
    }
}
