using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Options;
using Moq;
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
            public async Task CreateAsync_Maps_Posts_And_Completes_When_Api_Call_Succeeds()
            {
                // Arrange
                var workoutViewModel = new WorkoutViewModel {Name = "Test workout 01"};
                var workout = new Workout {Name = "Test workout 01"};

                MapperMock
                    .Setup(x => x.Map<WorkoutViewModel, Workout>(workoutViewModel))
                    .Returns(workout)
                    .Verifiable();
                HttpClientMock
                    .Setup(x => x.PostEntityAsync(It.IsAny<string>(), workout))
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.OK,
                        Content = new StringContent("Content as string")
                    })
                    .Verifiable();

                // Act
                await ServiceUnderTest.CreateAsync(workoutViewModel);

                // Assert
                MapperMock.Verify(x => x.Map<WorkoutViewModel, Workout>(workoutViewModel), Times.Once);
                HttpClientMock.Verify(x => x.PostEntityAsync(It.IsAny<string>(), workout), Times.Once);
            }

            [Fact]
            public async Task CreateAsync_Maps_Posts_And_Throws_HttpRequestException_When_Api_Call_Fails()
            {
                // Arrange
                var workoutViewModel = new WorkoutViewModel {Name = "Test workout 01"};
                var workout = new Workout {Name = "Test workout 01"};

                MapperMock
                    .Setup(x => x.Map<WorkoutViewModel, Workout>(workoutViewModel))
                    .Returns(workout)
                    .Verifiable();
                HttpClientMock
                    .Setup(x => x.PostEntityAsync(It.IsAny<string>(), workout))
                    .ReturnsAsync(new HttpResponseMessage
                    {
                        StatusCode = HttpStatusCode.InternalServerError,
                        Content = new StringContent("Content as string")
                    })
                    .Verifiable();

                // Act, Assert
                await Assert.ThrowsAsync<HttpRequestException>(() => ServiceUnderTest.CreateAsync(workoutViewModel));
                MapperMock.Verify(x => x.Map<WorkoutViewModel, Workout>(workoutViewModel), Times.Once);
                HttpClientMock.Verify(x => x.PostEntityAsync(It.IsAny<string>(), workout), Times.Once);
            }
        }
    }
}