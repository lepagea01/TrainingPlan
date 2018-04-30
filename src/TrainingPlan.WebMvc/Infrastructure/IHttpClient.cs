using System.Net.Http;
using System.Threading.Tasks;

namespace TrainingPlan.WebMvc.Infrastructure
{
    public interface IHttpClient
    {
        Task<string> GetStringAsync(string uri);
        Task<HttpResponseMessage> PostEntityAsync<T>(string uri, T entity);
    }
}