using TrainingPlan.ApplicationCore.Entities;
using TrainingPlan.ApplicationCore.Interfaces;

namespace TrainingPlan.Infrastructure.Data
{
    public class WorkoutRepository : EfRepository<Workout>, IWorkoutRepository
    {
        public WorkoutRepository(TrainingPlanContext trainingPlanContext) : base(trainingPlanContext)
        {
        }
    }
}