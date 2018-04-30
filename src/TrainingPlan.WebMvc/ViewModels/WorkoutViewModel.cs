using System.ComponentModel.DataAnnotations;

namespace TrainingPlan.WebMvc.ViewModels
{
    public class WorkoutViewModel
    {
        public int Id { get; set; }

        [Required] public string Name { get; set; }

        public string Description { get; set; }
    }
}