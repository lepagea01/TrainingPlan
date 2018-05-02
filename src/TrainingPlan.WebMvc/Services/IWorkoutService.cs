using System.Collections.Generic;
using System.Threading.Tasks;
using TrainingPlan.WebMvc.ViewModels;

namespace TrainingPlan.WebMvc.Services
{
    public interface IWorkoutService
    {
        Task CreateAsync(WorkoutViewModel workoutViewModel);
        Task<IEnumerable<WorkoutViewModel>> ReadAllAsync();
        Task<WorkoutViewModel> ReadOneAsync(int id);
        Task UpdateAsync(int id, WorkoutViewModel workoutViewModel);
        Task DeleteAsync(int id);
    }
}