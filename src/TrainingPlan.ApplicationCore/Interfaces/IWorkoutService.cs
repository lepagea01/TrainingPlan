using System.Collections.Generic;
using System.Threading.Tasks;
using TrainingPlan.ApplicationCore.Entities;

namespace TrainingPlan.ApplicationCore.Interfaces
{
    public interface IWorkoutService
    {
        Task<Workout> CreateAsync(Workout workout);
        Task<IEnumerable<Workout>> ReadAllAsync();
        Task<Workout> ReadOneAsync(int id);
        Task<Workout> UpdateAsync(int id, Workout workout);
        Task<Workout> DeleteAsync(int id);
    }
}