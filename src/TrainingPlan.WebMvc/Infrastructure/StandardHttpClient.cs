using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TrainingPlan.WebMvc.Infrastructure
{
    public class StandardHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public StandardHttpClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<HttpResponseMessage> PostEntityAsync<T>(string uri, T entity)
        {
            return await DoPostPutAsync(HttpMethod.Post, uri, entity);
        }

        public async Task<string> GetStringAsync(string uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await _httpClient.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> PutEntityAsync<T>(string uri, T entity)
        {
            return await DoPostPutAsync(HttpMethod.Put, uri, entity);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, uri);

            return await _httpClient.SendAsync(requestMessage);
        }

        private async Task<HttpResponseMessage> DoPostPutAsync<T>(HttpMethod httpMethod, string uri, T entity)
        {
            if (httpMethod != HttpMethod.Post && httpMethod != HttpMethod.Put)
                throw new ArgumentException("Value must be either post or put.", nameof(httpMethod));

            var requestMessage = new HttpRequestMessage(httpMethod, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(requestMessage);
            if (response.StatusCode == HttpStatusCode.InternalServerError) throw new HttpRequestException();

            return response;
        }
    }
}