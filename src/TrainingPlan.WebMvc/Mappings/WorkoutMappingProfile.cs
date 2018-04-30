using AutoMapper;
using TrainingPlan.ApplicationCore.Entities;
using TrainingPlan.WebMvc.ViewModels;

namespace TrainingPlan.WebMvc.Mappings
{
    public class WorkoutMappingProfile : Profile
    {
        public WorkoutMappingProfile()
        {
            CreateMap<Workout, WorkoutViewModel>();
            CreateMap<WorkoutViewModel, Workout>();
        }
    }
}