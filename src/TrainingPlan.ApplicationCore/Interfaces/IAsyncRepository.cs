using System.Collections.Generic;
using System.Threading.Tasks;
using TrainingPlan.ApplicationCore.Entities;

namespace TrainingPlan.ApplicationCore.Interfaces
{
    public interface IAsyncRepository<T> where T : BaseEntity
    {
        Task<T> CreateAsync(T entity);
        Task<IEnumerable<T>> ReadAllAsync();
        Task<T> ReadOneAsync(int id);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(int id);
    }
}