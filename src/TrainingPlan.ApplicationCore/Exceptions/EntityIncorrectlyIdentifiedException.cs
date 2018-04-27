using TrainingPlan.ApplicationCore.Interfaces;

namespace TrainingPlan.ApplicationCore.Exceptions
{
    public class EntityIncorrectlyIdentifiedException : TrainingPlanException
    {
        public EntityIncorrectlyIdentifiedException(int id, IBaseEntity entity)
            : base($"Entity {entity.Id} is incorrectly identified with {id}.")
        {
        }

        public EntityIncorrectlyIdentifiedException(int id, int entityId)
            : base($"Entity {entityId} is incorrectly identified with {id}.")
        {
        }
    }
}