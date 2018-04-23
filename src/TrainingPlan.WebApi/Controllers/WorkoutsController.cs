using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrainingPlan.ApplicationCore.Entities;
using TrainingPlan.ApplicationCore.Exceptions;
using TrainingPlan.ApplicationCore.Interfaces;

namespace TrainingPlan.WebApi.Controllers
{
    public class WorkoutsController : BaseController
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutsController(IWorkoutService workoutService)
        {
            _workoutService = workoutService ?? throw new ArgumentNullException(nameof(workoutService));
        }

        // GET: api/v1/workouts
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Workout>), StatusCodes.Status200OK)]
        public async Task<IActionResult> ReadAllAsync()
        {
            var workouts = await _workoutService.ReadAllAsync();
            return Ok(workouts);
        }

        // GET: api/v1/workouts/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Workout), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReadOneAsync([FromRoute] int id)
        {
            try
            {
                var workout = await _workoutService.ReadOneAsync(id);
                return Ok(workout);
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        // POST: api/v1/workouts
        [HttpPost]
        [ProducesResponseType(typeof(Workout), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] Workout workout)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var createdWorkout = await _workoutService.CreateAsync(workout);
            return CreatedAtAction(
                nameof(ReadOneAsync),
                new {id = createdWorkout.Id},
                createdWorkout);
        }

        // PUT: api/v1/workouts/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] Workout workout)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _workoutService.UpdateAsync(id, workout);
                return NoContent();
            }
            catch (EntityIncorrectlyIdentifiedException)
            {
                return BadRequest();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE: api/v1/workouts/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            try
            {
                await _workoutService.DeleteAsync(id);
                return NoContent();
            }
            catch (EntityNotFoundException)
            {
                return NotFound();
            }
        }
    }
}