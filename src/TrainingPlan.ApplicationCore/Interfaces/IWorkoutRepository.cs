using TrainingPlan.ApplicationCore.Entities;

namespace TrainingPlan.ApplicationCore.Interfaces
{
    public interface IWorkoutRepository : IAsyncRepository<Workout>, IRepository<Workout>
    {
    }
}