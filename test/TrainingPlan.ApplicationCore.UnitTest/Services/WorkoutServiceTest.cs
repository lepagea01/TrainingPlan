using System.Threading.Tasks;
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
            public async Task CreateAsync_Creates_And_Returns_Created_Workout()
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
            public async Task ReadAllAsync_Returns_All_Workouts()
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
            public async Task ReadOneAsync_Returns_A_Workout()
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
            public async Task ReadOneAsync_Throws_EntityNotFoundException_When_Workout_DoesNotExist()
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
            public async Task UpdateAsync_Enforces_Workout_Existence_And_Updates()
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
            public async Task
                UpdateAsync_Throws_EntityIncorrectlyIdentifiedException_When_Workout_Id_DoesNotMatchWorkoutId()
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
            public async Task UpdateAsync_Throws_EntityNotFoundException_When_WorkoutDoesNotExist()
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
            public async Task DeleteAsync_Enforces_Workout_Existence_And_Deletes()
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
            public async Task DeleteAsync_Throws_EntityNotFoundException_When_WorkoutDoesNotExist()
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