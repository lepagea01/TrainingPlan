using TrainingPlan.ApplicationCore.Interfaces;

namespace TrainingPlan.ApplicationCore.Entities
{
    public abstract class BaseEntity : ITrackable
    {
        public int Id { get; set; }
    }
}