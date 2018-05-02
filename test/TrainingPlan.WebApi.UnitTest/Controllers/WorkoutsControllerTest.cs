using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TrainingPlan.ApplicationCore.Entities;
using TrainingPlan.ApplicationCore.Exceptions;
using TrainingPlan.ApplicationCore.Interfaces;
using TrainingPlan.WebApi.Controllers;
using Xunit;

namespace TrainingPlan.WebApi.UnitTest.Controllers
{
    public class WorkoutsControllerTest
    {
        protected WorkoutsControllerTest()
        {
            WorkoutServiceMock = new Mock<IWorkoutService>();
            ControllerUnderTest = new WorkoutsController(WorkoutServiceMock.Object);
        }

        private WorkoutsController ControllerUnderTest { get; }
        private Mock<IWorkoutService> WorkoutServiceMock { get; }

        public class ReadAllAsync : WorkoutsControllerTest
        {
            [Fact]
            public async Task ReadAllAsync_Returns_OkObjectResult_With_Workouts()
            {
                // Arrange
                var expectedWorkouts = new[]
                {
                    new Workout {Name = "Test workout 01"},
                    new Workout {Name = "Test workout 02"},
                    new Workout {Name = "Test workout 03"}
                };
                WorkoutServiceMock
                    .Setup(x => x.ReadAllAsync())
                    .ReturnsAsync(expectedWorkouts);

                // Act
                var result = await ControllerUnderTest.ReadAllAsync();

                // Assert
                var okObjectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(expectedWorkouts, okObjectResult.Value);
            }
        }

        public class ReadOneAsync : WorkoutsControllerTest
        {
            [Fact]
            public async Task ReadOneAsync_Returns_NotFoundResult_When_EntityNotFoundException_Is_Thrown()
            {
                // Arrange
                const int id = 1;
                WorkoutServiceMock
                    .Setup(x => x.ReadOneAsync(id))
                    .ThrowsAsync(new EntityNotFoundException(id));

                // Act
                var result = await ControllerUnderTest.ReadOneAsync(id);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }

            [Fact]
            public async Task ReadOneAsync_Returns_OkObjectResult_With_Workout()
            {
                // Arrange
                const int id = 1;
                var expectedWorkout = new Workout {Name = "Test workout 01", Id = id};
                WorkoutServiceMock
                    .Setup(x => x.ReadOneAsync(id))
                    .ReturnsAsync(expectedWorkout);

                // Act
                var result = await ControllerUnderTest.ReadOneAsync(id);

                // Assert
                var okObjectResult = Assert.IsType<OkObjectResult>(result);
                Assert.Same(expectedWorkout, okObjectResult.Value);
            }
        }

        public class CreateAsync : WorkoutsControllerTest
        {
            [Fact]
            public async Task CreateAsync_Returns_BadRequestObjectResult_When_ModelStateIsInvalid()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};
                ControllerUnderTest.ModelState.AddModelError("Id", "Some error");

                // Act
                var result = await ControllerUnderTest.CreateAsync(expectedWorkout);

                // Assert
                var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequestObjectResult.Value);
            }

            [Fact]
            public async Task CreateAsync_Returns_CreatedAtActionResult_With_Workout()
            {
                // Arrange
                const int id = 1;
                var expectedWorkout = new Workout {Name = "Test workout 01", Id = id};
                const string expectedCreatedAtActionName = nameof(WorkoutsController.ReadOneAsync);
                WorkoutServiceMock
                    .Setup(x => x.CreateAsync(expectedWorkout))
                    .ReturnsAsync(() =>
                    {
                        expectedWorkout.Id = id;
                        return expectedWorkout;
                    });

                // Act
                var result = await ControllerUnderTest.CreateAsync(expectedWorkout);

                // Assert
                var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
                Assert.Same(expectedWorkout, createdAtActionResult.Value);
                Assert.Equal(expectedCreatedAtActionName, createdAtActionResult.ActionName);
                Assert.Equal(id, createdAtActionResult.RouteValues.GetValueOrDefault("id"));
            }
        }

        public class UpdateAsync : WorkoutsControllerTest
        {
            [Fact]
            public async Task UpdateAsync_Returns_BadRequestObjectResult_When_ModelStateIsInvalid()
            {
                // Arrange
                const int id = 1;
                var expectedWorkout = new Workout {Name = "Test workout 01", Id = id};
                ControllerUnderTest.ModelState.AddModelError("Id", "Some error");

                // Act
                var result = await ControllerUnderTest.UpdateAsync(id, expectedWorkout);

                // Assert
                var badRequestObjectResult = Assert.IsType<BadRequestObjectResult>(result);
                Assert.IsType<SerializableError>(badRequestObjectResult.Value);
            }

            [Fact]
            public async Task UpdateAsync_Returns_BadRequestResult_When_EntityIncorrectlyIdentifiedException_Is_Thrown()
            {
                // Arrange
                const int id = 1;
                var expectedWorkout = new Workout {Name = "Test workout 01", Id = id};
                WorkoutServiceMock
                    .Setup(x => x.UpdateAsync(id + 1, expectedWorkout))
                    .ThrowsAsync(new EntityIncorrectlyIdentifiedException(id + 1, expectedWorkout));

                // Act
                var result = await ControllerUnderTest.UpdateAsync(id + 1, expectedWorkout);

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }

            [Fact]
            public async Task UpdateAsync_Returns_NoContentResult()
            {
                // Arrange
                const int id = 1;
                var expectedWorkout = new Workout {Name = "Test workout 01", Id = id};
                WorkoutServiceMock
                    .Setup(x => x.UpdateAsync(id, expectedWorkout))
                    .ReturnsAsync(expectedWorkout);

                // Act
                var result = await ControllerUnderTest.UpdateAsync(id, expectedWorkout);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }

            [Fact]
            public async Task UpdateAsync_Returns_NotFoundResult_When_EntityNotFoundException_Is_Thrown()
            {
                // Arrange
                const int id = 1;
                var expectedWorkout = new Workout {Name = "Test workout 01", Id = id};
                WorkoutServiceMock
                    .Setup(x => x.UpdateAsync(id, expectedWorkout))
                    .ThrowsAsync(new EntityNotFoundException(expectedWorkout));

                // Act
                var result = await ControllerUnderTest.UpdateAsync(id, expectedWorkout);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }

        public class DeleteAsync : WorkoutsControllerTest
        {
            [Fact]
            public async Task DeleteAsync_Returns_NoContentResult()
            {
                // Arrange
                const int id = 1;
                var expectedWorkout = new Workout {Name = "Test workout 01", Id = id};
                WorkoutServiceMock
                    .Setup(x => x.DeleteAsync(id))
                    .ReturnsAsync(expectedWorkout);

                // Act
                var result = await ControllerUnderTest.DeleteAsync(id);

                // Assert
                Assert.IsType<NoContentResult>(result);
            }

            [Fact]
            public async Task DeleteAsync_Returns_NotFoundResult_When_EntityNotFoundException_Is_Thrown()
            {
                // Arrange
                const int id = 1;
                WorkoutServiceMock
                    .Setup(x => x.DeleteAsync(id))
                    .ThrowsAsync(new EntityNotFoundException(id));

                // Act
                var result = await ControllerUnderTest.DeleteAsync(id);

                // Assert
                Assert.IsType<NotFoundResult>(result);
            }
        }
    }
}