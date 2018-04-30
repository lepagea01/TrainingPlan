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

        public async Task<string> GetStringAsync(string uri)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
            var response = await _httpClient.SendAsync(requestMessage);
            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<HttpResponseMessage> PostEntityAsync<T>(string uri, T entity)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, uri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(entity), Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(requestMessage);
            if (response.StatusCode == HttpStatusCode.InternalServerError) throw new HttpRequestException();

            return response;
        }
    }
}