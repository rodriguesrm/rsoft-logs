﻿using System.Net.Http;

namespace RSoft.Logs.Providers
{

    /// <summary>
    /// Http client factory interface
    /// </summary>
    public class HttpClientFactory : IHttpClientFactory
    {

        ///<inheritdoc/>
        public HttpClient CreateClient(string name)
        {
            return new HttpClient();
        }
    }
}
