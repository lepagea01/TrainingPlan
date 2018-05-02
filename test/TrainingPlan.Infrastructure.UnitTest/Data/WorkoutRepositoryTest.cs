using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TrainingPlan.ApplicationCore.Entities;
using TrainingPlan.Infrastructure.Data;
using Xunit;

namespace TrainingPlan.Infrastructure.UnitTest.Data
{
    public class WorkoutRepositoryTest
    {
        protected WorkoutRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<TrainingPlanContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            TrainingPlanContextMock = new TrainingPlanContext(options);

            RepositoryUnderTest = new WorkoutRepository(TrainingPlanContextMock);
        }

        private WorkoutRepository RepositoryUnderTest { get; }
        private TrainingPlanContext TrainingPlanContextMock { get; }

        public class CreateAsync : WorkoutRepositoryTest
        {
            [Fact]
            public async Task CreateAsync_Creates_And_Returns_Workout()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};

                // Act
                var result = await RepositoryUnderTest.CreateAsync(expectedWorkout);

                // Assert
                Assert.Same(expectedWorkout, result);
            }
        }

        public class ReadAllAsync : WorkoutRepositoryTest
        {
            [Fact]
            public async Task ReadAllAsync_Returns_Workouts()
            {
                // Arrange
                var expectedWorkouts = new[]
                {
                    new Workout {Name = "Test workout 01"},
                    new Workout {Name = "Test workout 02"},
                    new Workout {Name = "Test workout 03"}
                };
                TrainingPlanContextMock.Workouts.AddRange(expectedWorkouts);
                await TrainingPlanContextMock.SaveChangesAsync();

                // Act
                var result = await RepositoryUnderTest.ReadAllAsync();

                // Assert
                Assert.Collection(result,
                    workout => Assert.Same(expectedWorkouts[0], workout),
                    workout => Assert.Same(expectedWorkouts[1], workout),
                    workout => Assert.Same(expectedWorkouts[2], workout)
                );
            }
        }

        public class ReadOneAsync : WorkoutRepositoryTest
        {
            [Fact]
            public async Task ReadOneAsync_Returns_Null_When_WorkoutDoesNotExist()
            {
                // Arrange
                const int id = 1;

                // Act
                var result = await RepositoryUnderTest.ReadOneAsync(id);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task ReadOneAsync_Returns_Workout()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};
                TrainingPlanContextMock.Workouts.Add(expectedWorkout);
                await TrainingPlanContextMock.SaveChangesAsync();
                var id = expectedWorkout.Id;

                // Act
                var result = await RepositoryUnderTest.ReadOneAsync(id);

                // Assert
                Assert.Same(expectedWorkout, result);
            }
        }

        public class DeleteAsync : WorkoutRepositoryTest
        {
            [Fact]
            public async Task DeleteAsync_Deletes_And_Returns_Workout()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};
                TrainingPlanContextMock.Workouts.Add(expectedWorkout);
                await TrainingPlanContextMock.SaveChangesAsync();
                var id = expectedWorkout.Id;

                // Act
                var result = await RepositoryUnderTest.DeleteAsync(id);

                // Assert
                Assert.Same(expectedWorkout, result);
            }

            [Fact]
            public async Task DeleteAsync_Returns_Null_When_WorkoutDoesNotExist()
            {
                // Arrange
                const int id = 1;

                // Act
                var result = await RepositoryUnderTest.DeleteAsync(id);

                // Assert
                Assert.Null(result);
            }
        }

        public class UpdateAsync : WorkoutRepositoryTest
        {
            [Fact]
            public async Task UpdateAsync_Returns_Null_When_Workout_DoesNotExist()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};

                // Act
                var result = await RepositoryUnderTest.UpdateAsync(expectedWorkout);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async Task UpdateAsync_Updates_And_Return_Workout()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};
                TrainingPlanContextMock.Workouts.Add(expectedWorkout);
                await TrainingPlanContextMock.SaveChangesAsync();

                // Act
                var result = await RepositoryUnderTest.UpdateAsync(expectedWorkout);

                // Assert
                Assert.Same(expectedWorkout, result);
            }
        }

        public class Create : WorkoutRepositoryTest
        {
            [Fact]
            public void Create_Creates_And_Returns_Workout()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};

                // Act
                var result = RepositoryUnderTest.Create(expectedWorkout);

                // Assert
                Assert.Same(expectedWorkout, result);
            }
        }

        public class ReadAll : WorkoutRepositoryTest
        {
            [Fact]
            public void ReadAll_Returns_Workouts()
            {
                // Arrange
                var expectedWorkouts = new[]
                {
                    new Workout {Name = "Test workout 01"},
                    new Workout {Name = "Test workout 02"},
                    new Workout {Name = "Test workout 03"}
                };
                TrainingPlanContextMock.Workouts.AddRange(expectedWorkouts);
                TrainingPlanContextMock.SaveChanges();

                // Act
                var result = RepositoryUnderTest.ReadAll();

                // Assert
                Assert.Collection(result,
                    workout => Assert.Same(expectedWorkouts[0], workout),
                    workout => Assert.Same(expectedWorkouts[1], workout),
                    workout => Assert.Same(expectedWorkouts[2], workout)
                );
            }
        }

        public class ReadOne : WorkoutRepositoryTest
        {
            [Fact]
            public void ReadOne_Returns_Null_When_WorkoutDoesNotExist()
            {
                // Arrange
                const int id = 1;

                // Act
                var result = RepositoryUnderTest.ReadOne(id);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public void ReadOne_Returns_Workout()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};
                TrainingPlanContextMock.Workouts.Add(expectedWorkout);
                TrainingPlanContextMock.SaveChanges();
                var id = expectedWorkout.Id;

                // Act
                var result = RepositoryUnderTest.ReadOne(id);

                // Assert
                Assert.Same(expectedWorkout, result);
            }
        }

        public class Update : WorkoutRepositoryTest
        {
            [Fact]
            public void Update_Returns_Null_When_WorkoutDoesNotExist()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};

                // Act
                var result = RepositoryUnderTest.Update(expectedWorkout);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public void Update_Updates_And_Returns_Workout()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};
                TrainingPlanContextMock.Workouts.Add(expectedWorkout);
                TrainingPlanContextMock.SaveChanges();

                // Act
                var result = RepositoryUnderTest.Update(expectedWorkout);

                // Assert
                Assert.Same(expectedWorkout, result);
            }
        }

        public class Delete : WorkoutRepositoryTest
        {
            [Fact]
            public void Delete_Deletes_And_Returns_Workout()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};
                TrainingPlanContextMock.Workouts.Add(expectedWorkout);
                TrainingPlanContextMock.SaveChanges();
                var id = expectedWorkout.Id;

                // Act
                var result = RepositoryUnderTest.Delete(id);

                // Assert
                Assert.Same(expectedWorkout, result);
            }

            [Fact]
            public void Delete_Returns_Null_When_WorkoutDoesNotExist()
            {
                // Arrange
                const int id = 1;

                // Act
                var result = RepositoryUnderTest.Delete(id);

                // Assert
                Assert.Null(result);
            }
        }
    }
}