using TrainingPlan.ApplicationCore.Entities;

namespace TrainingPlan.ApplicationCore.Exceptions
{
    public class EntityNotFoundException : TrainingPlanException
    {
        public EntityNotFoundException(BaseEntity entity)
            : base($"Entity {entity.Id} was not found.")
        {
        }

        public EntityNotFoundException(int id)
            : base($"Entity {id} was not found.")
        {
        }
    }
}