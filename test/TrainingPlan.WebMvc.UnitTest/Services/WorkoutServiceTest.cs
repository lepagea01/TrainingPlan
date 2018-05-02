using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
using Newtonsoft.Json;
using TrainingPlan.ApplicationCore.Entities;
using TrainingPlan.WebMvc.Infrastructure;
using TrainingPlan.WebMvc.Services;
using TrainingPlan.WebMvc.ViewModels;
using Xunit;

namespace TrainingPlan.WebMvc.UnitTest.Services
{
    public class WorkoutServiceTest
    {
        private readonly AppSettings _appSettings = new AppSettings
        {
            TrainingPlanApiUrl = "http://",
            TrainingPlanApiVersion = "123456"
        };

        protected WorkoutServiceTest()
        {
            MapperMock = new Mock<IMapper>();
            SettingsMock = new Mock<IOptionsSnapshot<AppSettings>>();
            SettingsMock
                .Setup(x => x.Value).Returns(_appSettings);
            HttpClientMock = new Mock<IHttpClient>();

            ServiceUnderTest = new WorkoutService(MapperMock.Object, SettingsMock.Object, HttpClientMock.Object);
        }

        private WorkoutService ServiceUnderTest { get; }
        private Mock<IMapper> MapperMock { get; }
        private Mock<IOptionsSnapshot<AppSettings>> SettingsMock { get; }
        private Mock<IHttpClient> HttpClientMock { get; }

        public class CreateAsync : WorkoutServiceTest
        {
            [Fact]
            public async Task CreateAsync_Maps_Posts_And_Completes_When_ApiCall_Succeeds()
            {
                // Arrange
                var expectedWorkoutViewModel = new WorkoutViewModel {Name = "Test workout 01"};
                var expectedWorkout = new Workout {Name = "Test workout 01"};

                MapperMock
                    .Setup(x => x.Map<WorkoutViewModel, Workout>(expectedWorkoutViewModel))
                    .Returns(expectedWorkout)
                    .Verifiable();
                HttpClientMock
                    .Setup(x => x.PostEntityAsync(It.IsAny<string>(), expectedWorkout))
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent("Content as string")
                    })
                    .Verifiable();

                // Act
                await ServiceUnderTest.CreateAsync(expectedWorkoutViewModel);

                // Assert
                MapperMock.Verify(x => x.Map<WorkoutViewModel, Workout>(expectedWorkoutViewModel), Times.Once);
                HttpClientMock.Verify(x => x.PostEntityAsync(It.IsAny<string>(), expectedWorkout), Times.Once);
            }

            [Fact]
            public async Task CreateAsync_Maps_Posts_And_Throws_HttpRequestException_When_ApiCall_Fails()
            {
                // Arrange
                var expectedWorkoutViewModel = new WorkoutViewModel {Name = "Test workout 01"};
                var expectedWorkout = new Workout {Name = "Test workout 01"};

                MapperMock
                    .Setup(x => x.Map<WorkoutViewModel, Workout>(expectedWorkoutViewModel))
                    .Returns(expectedWorkout)
                    .Verifiable();
                HttpClientMock
                    .Setup(x => x.PostEntityAsync(It.IsAny<string>(), expectedWorkout))
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Content = new StringContent("Content as string")
                    })
                    .Verifiable();

                // Act, Assert
                await Assert.ThrowsAsync<HttpRequestException>(() =>
                    ServiceUnderTest.CreateAsync(expectedWorkoutViewModel));
                MapperMock.Verify(x => x.Map<WorkoutViewModel, Workout>(expectedWorkoutViewModel), Times.Once);
                HttpClientMock.Verify(x => x.PostEntityAsync(It.IsAny<string>(), expectedWorkout), Times.Once);
            }
        }

        public class ReadAllAsync : WorkoutServiceTest
        {
            [Fact]
            public async Task ReadAllAsync_Maps_Gets_And_Returns_All_Workouts_When_ApiCall_Returns_Workouts()
            {
                // Arrange
                var expectedWorkouts = new[]
                {
                    new Workout {Name = "Test workout 01", Id = 1},
                    new Workout {Name = "Test workout 02", Id = 2},
                    new Workout {Name = "Test workout 03", Id = 3}
                };
                var expectedWorkoutsViewModel = new[]
                {
                    new WorkoutViewModel {Name = "Test workout 01", Id = 1},
                    new WorkoutViewModel {Name = "Test workout 02", Id = 2},
                    new WorkoutViewModel {Name = "Test workout 03", Id = 3}
                };

                HttpClientMock
                    .Setup(x => x.GetStringAsync(It.IsAny<string>()))
                    .ReturnsAsync(JsonConvert.SerializeObject(expectedWorkouts))
                    .Verifiable();
                MapperMock
                    .Setup(x =>
                        x.Map<IEnumerable<Workout>, IEnumerable<WorkoutViewModel>>(It.IsAny<IEnumerable<Workout>>()))
                    .Returns(expectedWorkoutsViewModel)
                    .Verifiable();

                // Act
                var result = await ServiceUnderTest.ReadAllAsync();

                // Assert
                Assert.Same(expectedWorkoutsViewModel, result);
                MapperMock.Verify(
                    x => x.Map<IEnumerable<Workout>, IEnumerable<WorkoutViewModel>>(It.IsAny<IEnumerable<Workout>>()),
                    Times.Once);
                HttpClientMock.Verify(x => x.GetStringAsync(It.IsAny<string>()), Times.Once);
            }
        }

        public class ReadOneAsync : WorkoutServiceTest
        {
            [Fact]
            public async Task ReadOneAsync_Gets_DoesNotMap_And_Returns_Null_When_ApiCall_Returns_Null()
            {
                // Arrange
                const int id = 1;
                HttpClientMock
                    .Setup(x => x.GetStringAsync(It.IsAny<string>()))
                    .ReturnsAsync((string) null)
                    .Verifiable();
                MapperMock
                    .Setup(x => x.Map<Workout, WorkoutViewModel>(It.IsAny<Workout>()))
                    .Returns(It.IsAny<WorkoutViewModel>())
                    .Verifiable();

                // Act
                var result = await ServiceUnderTest.ReadOneAsync(id);

                // Assert
                Assert.Null(result);
                HttpClientMock.Verify(x => x.GetStringAsync(It.IsAny<string>()), Times.Once);
                MapperMock.Verify(x => x.Map<Workout, WorkoutViewModel>(It.IsAny<Workout>()), Times.Never);
            }

            [Fact]
            public async Task ReadOneAsync_Gets_Maps_And_Returns_A_Workout_When_ApiCall_Returns_Workout()
            {
                // Arrange
                const int id = 1;
                var expectedWorkout = new Workout {Name = "Test workout 01", Id = id};
                var expectedWorkoutViewModel = new WorkoutViewModel {Name = "Test workout 01", Id = id};

                HttpClientMock
                    .Setup(x => x.GetStringAsync(It.IsAny<string>()))
                    .ReturnsAsync(JsonConvert.SerializeObject(expectedWorkout))
                    .Verifiable();
                MapperMock
                    .Setup(x => x.Map<Workout, WorkoutViewModel>(It.IsAny<Workout>()))
                    .Returns(expectedWorkoutViewModel)
                    .Verifiable();

                // Act
                var result = await ServiceUnderTest.ReadOneAsync(id);

                // Assert
                Assert.Same(expectedWorkoutViewModel, result);
                HttpClientMock.Verify(x => x.GetStringAsync(It.IsAny<string>()), Times.Once);
                MapperMock.Verify(x => x.Map<Workout, WorkoutViewModel>(It.IsAny<Workout>()), Times.Once);
            }
        }

        public class UpdateAsync : WorkoutServiceTest
        {
            [Fact]
            public async Task UpdateAsync_Maps_Puts_And_Throws_HttpRequestException_When_ApiCall_Fails()
            {
                // Arrange
                const int id = 1;
                var expectedWorkoutViewModel = new WorkoutViewModel {Name = "Test workout 01", Id = id};
                var expectedWorkout = new Workout {Name = "Test workout 01", Id = id};

                MapperMock
                    .Setup(x => x.Map<WorkoutViewModel, Workout>(expectedWorkoutViewModel))
                    .Returns(expectedWorkout)
                    .Verifiable();
                HttpClientMock
                    .Setup(x => x.PutEntityAsync(It.IsAny<string>(), expectedWorkout))
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Content = new StringContent("Content as string")
                    })
                    .Verifiable();

                // Act, Assert
                await Assert.ThrowsAsync<HttpRequestException>(() =>
                    ServiceUnderTest.UpdateAsync(id, expectedWorkoutViewModel));
                MapperMock.Verify(x => x.Map<WorkoutViewModel, Workout>(expectedWorkoutViewModel), Times.Once);
                HttpClientMock.Verify(x => x.PutEntityAsync(It.IsAny<string>(), expectedWorkout), Times.Once);
            }

            [Fact]
            public async Task UpdateSync_Maps_Puts_And_Completes_When_ApiCall_Succeds()
            {
                // Arrange
                const int id = 1;
                var expectedWorkoutViewModel = new WorkoutViewModel {Name = "Test workout 01", Id = id};
                var expectedWorkout = new Workout {Name = "Test workout 01", Id = id};

                MapperMock
                    .Setup(x => x.Map<WorkoutViewModel, Workout>(expectedWorkoutViewModel))
                    .Returns(expectedWorkout)
                    .Verifiable();
                HttpClientMock
                    .Setup(x => x.PutEntityAsync(It.IsAny<string>(), expectedWorkout))
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent("Content as string")
                    })
                    .Verifiable();

                // Act
                await ServiceUnderTest.UpdateAsync(id, expectedWorkoutViewModel);

                // Assert
                MapperMock.Verify(x => x.Map<WorkoutViewModel, Workout>(expectedWorkoutViewModel), Times.Once);
                HttpClientMock.Verify(x => x.PutEntityAsync(It.IsAny<string>(), expectedWorkout), Times.Once);
            }
        }

        public class DeleteAsync : WorkoutServiceTest
        {
            [Fact]
            public async Task DeleteAsync_Deletes_And_Completes_When_ApiCall_Succeeds()
            {
                // Arrange
                const int id = 1;
                HttpClientMock
                    .Setup(x => x.DeleteAsync(It.IsAny<string>()))
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent("Content as string")
                    })
                    .Verifiable();

                // Act
                await ServiceUnderTest.DeleteAsync(id);

                // Assert
                HttpClientMock.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Once);
            }

            [Fact]
            public async Task DeleteAsync_Deletes_And_Throws_HttpRequestException_When_ApiCall_Fails()
            {
                // Arrange
                const int id = 1;

                HttpClientMock
                    .Setup(x => x.DeleteAsync(It.IsAny<string>()))
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Content = new StringContent("Content as string")
                    })
                    .Verifiable();

                // Act, Assert
                await Assert.ThrowsAsync<HttpRequestException>(() =>
                    ServiceUnderTest.DeleteAsync(id));
                HttpClientMock.Verify(x => x.DeleteAsync(It.IsAny<string>()), Times.Once);
            }
        }
    }
}