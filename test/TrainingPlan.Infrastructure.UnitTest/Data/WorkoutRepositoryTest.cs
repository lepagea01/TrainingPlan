using System;
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
            public async void Should_create_and_return_the_created_workout()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};

                // Act
                var result = await RepositoryUnderTest.CreateAsync(expectedWorkout);

                // Assert
                Assert.Same(expectedWorkout, result);
            }
        }

        public class DeleteAsync : WorkoutRepositoryTest
        {
            [Fact]
            public async void Should_delete_and_return_the_deleted_workout()
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
            public async void Should_return_null_when_the_workout_does_not_exist()
            {
                // Arrange
                const int id = 1;

                // Act
                var result = await RepositoryUnderTest.DeleteAsync(id);

                // Assert
                Assert.Null(result);
            }
        }

        public class ReadAllAsync : WorkoutRepositoryTest
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
            public async void Should_return_a_workout()
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

            [Fact]
            public async void Should_return_null_when_the_workout_does_not_exist()
            {
                // Arrange
                const int id = 1;

                // Act
                var result = await RepositoryUnderTest.ReadOneAsync(id);

                // Assert
                Assert.Null(result);
            }
        }

        public class UpdateAsync : WorkoutRepositoryTest
        {
            [Fact]
            public async void Should_return_null_when_the_workout_does_not_exist()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};

                // Act
                var result = await RepositoryUnderTest.UpdateAsync(expectedWorkout);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public async void Should_update_and_return_the_updated_workout()
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
            public void Should_create_and_return_the_created_workout()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};

                // Act
                var result = RepositoryUnderTest.Create(expectedWorkout);

                // Assert
                Assert.Same(expectedWorkout, result);
            }
        }

        public class Delete : WorkoutRepositoryTest
        {
            [Fact]
            public void Should_delete_and_return_the_deleted_workout()
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
            public void Should_return_null_when_the_workout_does_not_exist()
            {
                // Arrange
                const int id = 1;

                // Act
                var result = RepositoryUnderTest.Delete(id);

                // Assert
                Assert.Null(result);
            }
        }

        public class ReadAll : WorkoutRepositoryTest
        {
            [Fact]
            public void Should_return_all_workouts()
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
            public void Should_return_a_workout()
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

            [Fact]
            public void Should_return_null_when_the_workout_does_not_exist()
            {
                // Arrange
                const int id = 1;

                // Act
                var result = RepositoryUnderTest.ReadOne(id);

                // Assert
                Assert.Null(result);
            }
        }

        public class Update : WorkoutRepositoryTest
        {
            [Fact]
            public void Should_return_null_when_the_workout_does_not_exist()
            {
                // Arrange
                var expectedWorkout = new Workout {Name = "Test workout 01"};

                // Act
                var result = RepositoryUnderTest.Update(expectedWorkout);

                // Assert
                Assert.Null(result);
            }

            [Fact]
            public void Should_update_and_return_the_updated_workout()
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
    }
}