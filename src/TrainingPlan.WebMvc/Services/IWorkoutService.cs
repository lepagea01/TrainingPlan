using System.Collections.Generic;
using System.Threading.Tasks;
using TrainingPlan.WebMvc.ViewModels;

namespace TrainingPlan.WebMvc.Services
{
    public interface IWorkoutService
    {
        Task CreateAsync(WorkoutViewModel workoutViewModel);
        Task<IEnumerable<WorkoutViewModel>> GetAllAsync();
        Task<WorkoutViewModel> GetByIdAsync(int id);
    }
}