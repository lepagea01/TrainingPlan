using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingPlan.ApplicationCore.Entities;

namespace TrainingPlan.Infrastructure.Data
{
    public static class TrainingPlanContextSeed
    {
        public static async Task SeedAsync(TrainingPlanContext trainingPlanContext)
        {
            if (!trainingPlanContext.Workouts.Any())
            {
                trainingPlanContext.Workouts.AddRange(GetPreconfiguredWorkouts());

                await trainingPlanContext.SaveChangesAsync();
            }
        }

        private static IEnumerable<Workout> GetPreconfiguredWorkouts()
        {
            return new[]
            {
                new Workout {Name = "ei75"},
                new Workout {Name = "ei100"}
            };
        }
    }
}