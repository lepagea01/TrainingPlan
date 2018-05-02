using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrainingPlan.ApplicationCore.Entities;
using TrainingPlan.ApplicationCore.Exceptions;
using TrainingPlan.ApplicationCore.Interfaces;

namespace TrainingPlan.ApplicationCore.Services
{
    public class WorkoutService : IWorkoutService
    {
        private readonly IWorkoutRepository _workoutRepository;

        public WorkoutService(IWorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository ?? throw new ArgumentNullException(nameof(workoutRepository));
        }

        public async Task<Workout> CreateAsync(Workout workout)
        {
            return await _workoutRepository.CreateAsync(workout);
        }

        public async Task<IEnumerable<Workout>> ReadAllAsync()
        {
            return await _workoutRepository.ReadAllAsync();
        }

        public async Task<Workout> ReadOneAsync(int id)
        {
            return await EnforceWorkoutExistenceAsync(id);
        }

        public async Task<Workout> UpdateAsync(int id, Workout workout)
        {
            Guard.AgainstEntityIncorrectlyIdentified(id, workout);
            await EnforceWorkoutExistenceAsync(workout.Id);
            return await _workoutRepository.UpdateAsync(workout);
        }

        public async Task<Workout> DeleteAsync(int id)
        {
            await EnforceWorkoutExistenceAsync(id);
            return await _workoutRepository.DeleteAsync(id);
        }

        private async Task<Workout> EnforceWorkoutExistenceAsync(int id)
        {
            var workout = await _workoutRepository.ReadOneAsync(id);
            Guard.AgainstEntityNotFound(id, workout);

            return workout;
        }
    }
}