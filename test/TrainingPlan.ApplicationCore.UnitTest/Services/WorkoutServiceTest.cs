using Moq;
using TrainingPlan.ApplicationCore.Entities;
using TrainingPlan.ApplicationCore.Exceptions;
using TrainingPlan.ApplicationCore.Interfaces;
using TrainingPlan.ApplicationCore.Services;
using Xunit;

namespace TrainingPlan.ApplicationCore.UnitTest.Services
{
    public class WorkoutServiceTest
    {
        protected WorkoutServiceTest()
        {
            WorkoutRepositoryMock = new Mock<IWorkoutRepository>();
            ServiceUnderTest = new WorkoutService(WorkoutRepositoryMock.Object);
        }

        private WorkoutService ServiceUnderTest { get; }
        private Mock<IWorkoutRepository> WorkoutRepositoryMock { get; }

        public class CreateAsync : WorkoutServiceTest
        {
            [Fact]
            public async void Should_create_and_return_the_created_workout()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};
                WorkoutRepositoryMock
                    .Setup(x => x.CreateAsync(expectedWorkout))
                    .ReturnsAsync(expectedWorkout)
                    .Verifiable();

                // Act
                var result = await ServiceUnderTest.CreateAsync(expectedWorkout);

                // Assert
                Assert.Same(expectedWorkout, result);
                WorkoutRepositoryMock.Verify(x => x.CreateAsync(expectedWorkout), Times.Once);
            }
        }

        public class ReadAllAsync : WorkoutServiceTest
        {
            [Fact]
            public async void Should_return_all_workouts()
            {
                // Arrange
                var expectedWorkouts = new[]
                {
                    new Workout {Name = "Test workout 01"},
                    new Workout {Name = "Test workout 02"},
                    new Workout {Name = "Test workout 03"}
                };
                WorkoutRepositoryMock
                    .Setup(x => x.ReadAllAsync())
                    .ReturnsAsync(expectedWorkouts);

                // Act
                var result = await ServiceUnderTest.ReadAllAsync();

                // Assert
                Assert.Same(expectedWorkouts, result);
            }
        }

        public class ReadOneAsync : WorkoutServiceTest
        {
            [Fact]
            public async void Should_return_a_workout()
            {
                // Arrange
                const int id = 1;
                var expectedWorkout = new Workout {Name = "Test workout 01", Id = id};
                WorkoutRepositoryMock
                    .Setup(x => x.ReadOneAsync(id))
                    .ReturnsAsync(expectedWorkout);

                //Act
                var result = await ServiceUnderTest.ReadOneAsync(id);

                //Assert
                Assert.Same(expectedWorkout, result);
            }

            [Fact]
            public async void Should_throw_EntityNotFoundException_when_workout_does_not_exist()
            {
                // Arrange
                const int id = 1;
                WorkoutRepositoryMock
                    .Setup(x => x.ReadOneAsync(id))
                    .ReturnsAsync(default(Workout));

                // Act, Assert
                await Assert.ThrowsAsync<EntityNotFoundException>(() => ServiceUnderTest.ReadOneAsync(id));
            }
        }

        public class UpdateAsync : WorkoutServiceTest
        {
            [Fact]
            public async void Should_enforce_workout_existence_and_update()
            {
                // Arrange
                const int id = 1;
                var expectedWorkout = new Workout {Name = "Test workout 01", Id = id};
                WorkoutRepositoryMock
                    .Setup(x => x.ReadOneAsync(id))
                    .ReturnsAsync(expectedWorkout)
                    .Verifiable();
                WorkoutRepositoryMock
                    .Setup(x => x.UpdateAsync(expectedWorkout))
                    .ReturnsAsync(expectedWorkout)
                    .Verifiable();

                // Act, Assert
                await ServiceUnderTest.UpdateAsync(id, expectedWorkout);
                WorkoutRepositoryMock.Verify(x => x.ReadOneAsync(id), Times.Once);
                WorkoutRepositoryMock.Verify(x => x.UpdateAsync(expectedWorkout), Times.Once);
            }

            [Fact]
            public async void
                Should_throw_EntityIncorrectlyIdentifiedException_when_workout_id_does_not_match_workoutId()
            {
                const int id = 1;
                var expectedWorkout = new Workout {Name = "Test workout 01", Id = id};
                WorkoutRepositoryMock
                    .Setup(x => x.ReadOneAsync(id))
                    .ReturnsAsync(default(Workout))
                    .Verifiable();
                WorkoutRepositoryMock
                    .Setup(x => x.UpdateAsync(expectedWorkout))
                    .Verifiable();

                // Act, Assert
                await Assert.ThrowsAsync<EntityIncorrectlyIdentifiedException>(() =>
                    ServiceUnderTest.UpdateAsync(id + 1, expectedWorkout));
                WorkoutRepositoryMock.Verify(x => x.ReadOneAsync(id), Times.Never);
                WorkoutRepositoryMock.Verify(x => x.UpdateAsync(expectedWorkout), Times.Never);
            }

            [Fact]
            public async void Should_throw_EntityNotFoundException_when_workout_does_not_exist()
            {
                // Arrange
                const int id = 1;
                var expectedWorkout = new Workout {Name = "Test workout 01", Id = id};
                WorkoutRepositoryMock
                    .Setup(x => x.ReadOneAsync(id))
                    .ReturnsAsync(default(Workout))
                    .Verifiable();
                WorkoutRepositoryMock
                    .Setup(x => x.UpdateAsync(expectedWorkout))
                    .Verifiable();

                // Act, Assert
                await Assert.ThrowsAsync<EntityNotFoundException>(() =>
                    ServiceUnderTest.UpdateAsync(id, expectedWorkout));
                WorkoutRepositoryMock.Verify(x => x.ReadOneAsync(id), Times.Once);
                WorkoutRepositoryMock.Verify(x => x.UpdateAsync(expectedWorkout), Times.Never);
            }
        }

        public class DeleteAsync : WorkoutServiceTest
        {
            [Fact]
            public async void Should_enforce_workout_existence_and_delete()
            {
                // Arrange
                const int id = 1;
                var expectedWorkout = new Workout {Name = "Test workout 01", Id = id};
                WorkoutRepositoryMock
                    .Setup(x => x.ReadOneAsync(id))
                    .ReturnsAsync(expectedWorkout)
                    .Verifiable();
                WorkoutRepositoryMock
                    .Setup(x => x.DeleteAsync(id))
                    .ReturnsAsync(expectedWorkout)
                    .Verifiable();

                // Act, Assert
                await ServiceUnderTest.DeleteAsync(id);
                WorkoutRepositoryMock.Verify(x => x.ReadOneAsync(id), Times.Once);
                WorkoutRepositoryMock.Verify(x => x.DeleteAsync(id), Times.Once);
            }

            [Fact]
            public async void Should_throw_EntityNotFoundException_when_workout_does_not_exist()
            {
                // Arrange
                const int id = 1;
                WorkoutRepositoryMock
                    .Setup(x => x.ReadOneAsync(id))
                    .ReturnsAsync(default(Workout))
                    .Verifiable();
                WorkoutRepositoryMock
                    .Setup(x => x.DeleteAsync(id))
                    .Verifiable();

                // Act, Assert
                await Assert.ThrowsAsync<EntityNotFoundException>(() => ServiceUnderTest.DeleteAsync(id));
                WorkoutRepositoryMock.Verify(x => x.ReadOneAsync(id), Times.Once);
                WorkoutRepositoryMock.Verify(x => x.DeleteAsync(id), Times.Never);
            }
        }
    }
}