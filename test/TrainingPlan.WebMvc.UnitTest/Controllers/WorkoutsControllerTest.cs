using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TrainingPlan.WebMvc.Controllers;
using TrainingPlan.WebMvc.Services;
using TrainingPlan.WebMvc.ViewModels;
using Xunit;

namespace TrainingPlan.WebMvc.UnitTest.Controllers
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

        public class Index : WorkoutsControllerTest
        {
            [Fact]
            public async Task Get_Returns_ViewResult_With_Workouts()
            {
                // Arrange
                var expectedWorkouts = new[]
                {
                    new WorkoutViewModel {Name = "Test workout 01"},
                    new WorkoutViewModel {Name = "Test workout 02"},
                    new WorkoutViewModel {Name = "Test workout 03"}
                };
                WorkoutServiceMock
                    .Setup(x => x.GetAllAsync())
                    .ReturnsAsync(expectedWorkouts);

                // Act
                var result = await ControllerUnderTest.Index();

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Same(expectedWorkouts, viewResult.Model);
            }
        }

        public class Edit : WorkoutsControllerTest
        {
            [Fact]
            public async Task Get_Returns_ViewResult_With_Workout_When_WorkoutIsFound()
            {
                // Arrange
                const int id = 1;
                var expectedWorkout = new WorkoutViewModel {Name = "Test workout 01", Id = id};
                WorkoutServiceMock
                    .Setup(x => x.GetByIdAsync(id))
                    .ReturnsAsync(expectedWorkout);

                // Act
                var result = await ControllerUnderTest.Edit(id);

                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Same(expectedWorkout, viewResult.Model);
            }

            [Fact]
            public async Task Get_Returns_NotFoundResult_When_WorkoutIsNotFound()
            {
                // Arrange
                const int id = 1;
                WorkoutServiceMock
                    .Setup(x => x.GetByIdAsync(id))
                    .ReturnsAsync(default(WorkoutViewModel));

                // Act
                var result = await ControllerUnderTest.Edit(id);

                // Assert
                Assert.IsType<NotFoundResult>(result);
                
            }
        }

        public class Create : WorkoutsControllerTest
        {
            [Fact]
            public async Task Get_Returns_ViewResult_With_DefaultWorkout()
            {
                // Arrange
                var expectedWork = default(WorkoutViewModel);
                
                // Act
                var result = await ControllerUnderTest.Create();
                
                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Same(expectedWork, viewResult.Model);
            }
            
            [Fact]
            public async Task Post_Returns_ViewResult_When_ModelStateIsInvalid()
            {
                // Arrange
                var expectedWorkout = new WorkoutViewModel {Name = "Test workout 01"};
                ControllerUnderTest.ModelState.AddModelError("Id", "Some error");
                
                // Act
                var result = await ControllerUnderTest.Create(expectedWorkout);
                
                // Assert
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.IsType<WorkoutViewModel>(viewResult.Model);
            }

            [Fact]
            public async Task Post_Returns_RedirectToActionResult_When_ModelStateIsValid()
            {
                // Arrange
                const int id = 1;
                var expectedWorkout = new WorkoutViewModel {Name = "Test workout 01", Id = id};
                const string expectedActionName = nameof(WorkoutsController.Index);
                WorkoutServiceMock
                    .Setup(x => x.CreateAsync(expectedWorkout))
                    .Returns(Task.CompletedTask);
                
                // Act
                var result = await ControllerUnderTest.Create(expectedWorkout);
                
                // Assert
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal(expectedActionName, redirectToActionResult.ActionName);
            }
        }
    }
}