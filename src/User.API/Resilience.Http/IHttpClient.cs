using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Resilience.Http
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> PostAsync<T>(string uri, T item, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer");
        Task<HttpResponseMessage> PostAsync(string uri, Dictionary<string,string> form, string authorizationToken = null, string requestId = null, string authorizationMethod = "Bearer");
    }
}
