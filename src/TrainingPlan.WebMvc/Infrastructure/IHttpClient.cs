using System.Net.Http;
using System.Threading.Tasks;

namespace TrainingPlan.WebMvc.Infrastructure
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> PostEntityAsync<T>(string uri, T entity);
        Task<string> GetStringAsync(string uri);
        Task<HttpResponseMessage> PutEntityAsync<T>(string uri, T entity);
        Task<HttpResponseMessage> DeleteAsync(string uri);
    }
}