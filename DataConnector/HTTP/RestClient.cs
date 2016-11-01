using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DataConnector.HTTP
{
    class RequestHeader
    {
        public string HeaderKey { get; set; }
        public string HeaderValue { get; set; }
    }
    class RestClient : IDisposable
    {
        private readonly HttpClient Client = null;

        public RestClient()
        {
            Client = new HttpClient();
        }

        public async Task<HttpResponseMessage> GetBodyAsync(string url, string data)
        {
            HttpContent Content = new StringContent(data);
            Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = Encoding.UTF8.WebName };

            HttpResponseMessage ResponseMessage = await Client.GetAsync(new Uri(url));

            return ResponseMessage;
        }

        public async Task<HttpResponseMessage> GetBodyAsync(string url, string data, RequestHeader[] headers)
        {
            lock (Client)
            {
                foreach (RequestHeader Header in headers)
                    Client.DefaultRequestHeaders.Add(Header.HeaderKey, Header.HeaderValue);
            }

            HttpResponseMessage ResponseMessage = await GetBodyAsync(url, data);
            Client.DefaultRequestHeaders.Clear();

            return ResponseMessage;
        }

        public async Task<HttpResponseMessage> GetBodyAsync(string url, string data, RequestHeader[] headers,
          MediaTypeWithQualityHeaderValue[] acceptHeaders)
        {
            lock (Client)
            {
                foreach (var AcceptHeader in acceptHeaders)
                    Client.DefaultRequestHeaders.Accept.Add(AcceptHeader);
            }


            HttpResponseMessage ResponseMessage = await GetBodyAsync(url, data, headers);

            Client.DefaultRequestHeaders.Accept.Clear();

            return ResponseMessage;
        }

        public async Task<HttpResponseMessage> GetBodyAsync(string url, string data,
            MediaTypeWithQualityHeaderValue[] acceptHeaders)
        {
            lock (Client)
            {
                foreach (var AcceptHeader in acceptHeaders)
                    Client.DefaultRequestHeaders.Accept.Add(AcceptHeader);
            }

            HttpResponseMessage ResponseMessage = await GetBodyAsync(url, data);

            Client.DefaultRequestHeaders.Accept.Clear();

            return ResponseMessage;
        }

        public async Task<HttpResponseMessage> GetJsonAsync(string url, string data)
        {
            return await GetBodyAsync(url, data,
                new[]
                {
                    new MediaTypeWithQualityHeaderValue("application/json") { CharSet = Encoding.UTF8.WebName }
                });
        }

        public async Task<HttpResponseMessage> PostBodyAsync(string url, string data)
        {
            HttpContent Content = new StringContent(data);
            Content.Headers.ContentType = 
                new MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = Encoding.UTF8.WebName };

            HttpResponseMessage ResponseMessage = await Client.PostAsync(new Uri(url), Content);

            return ResponseMessage;
        }

        public async Task<HttpResponseMessage> PostBodyAsync(string url, string data, RequestHeader[] headers)
        {
            lock (Client)
            {
                foreach (RequestHeader Header in headers)
                    Client.DefaultRequestHeaders.Add(Header.HeaderKey, Header.HeaderValue);
            }


            HttpResponseMessage ResponseMessage = await PostBodyAsync(url, data);
            Client.DefaultRequestHeaders.Clear();

            return ResponseMessage;
        }

        public async Task<HttpResponseMessage> PostBodyAsync(string url, string data, RequestHeader[] headers,
            MediaTypeWithQualityHeaderValue[] acceptHeaders)
        {
            lock (Client)
            {
                foreach (var AcceptHeader in acceptHeaders)
                    Client.DefaultRequestHeaders.Accept.Add(AcceptHeader);
            }

            HttpResponseMessage ResponseMessage = await PostBodyAsync(url, data, headers);

            Client.DefaultRequestHeaders.Accept.Clear();

            return ResponseMessage;
        }

        public async Task<HttpResponseMessage> PostBodyAsync(string url, string data,
            MediaTypeWithQualityHeaderValue[] acceptHeaders)
        {
            lock (Client)
            {
                foreach (var AcceptHeader in acceptHeaders)
                    Client.DefaultRequestHeaders.Accept.Add(AcceptHeader);
            }

            HttpResponseMessage ResponseMessage = await PostBodyAsync(url, data);

            Client.DefaultRequestHeaders.Accept.Clear();

            return ResponseMessage;
        }

        public async Task<HttpResponseMessage> PostJsonAsync(string url, string data)
        {
            return await PostBodyAsync(url, data, 
                new []
                {
                    new MediaTypeWithQualityHeaderValue("application/json") { CharSet = Encoding.UTF8.WebName }
                });
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
