using TrainingPlan.ApplicationCore.Entities;

namespace TrainingPlan.ApplicationCore.Exceptions
{
    public static class Guard
    {
        public static void AgainstEntityNotFound(int id, BaseEntity entity)
        {
            if (entity == null) throw new EntityNotFoundException(id);
        }

        public static void AgainstEntityIncorrectlyIdentified(int id, BaseEntity entity)
        {
            if (id != entity.Id) throw new EntityIncorrectlyIdentifiedException(id, entity.Id);
        }
    }
}