using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace TrainingPlan.WebMvc.Infrastructure
{
    public class StandardHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public StandardHttpClient()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetStringAsync(string uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await _httpClient.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> PostEntityAsync<T>(string uri, T entity)
        {
            throw new NotImplementedException();
        }
    }
}