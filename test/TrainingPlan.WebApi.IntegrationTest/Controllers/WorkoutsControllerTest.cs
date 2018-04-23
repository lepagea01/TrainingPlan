using System.Threading.Tasks;
using TrainingPlan.WebApi.IntegrationTest.Shared;
using Xunit;

namespace TrainingPlan.WebApi.IntegrationTest.Controllers
{
    public class WorkoutsControllerTest : BaseHttpTest
    {
        public class ReadAllAsync : WorkoutsControllerTest
        {
            [Fact]
            public async Task Should_return_response_ok_status_code()
            {
                // Arrange

                // Act
                var result = await Client.GetAsync("api/v1/workouts");

                // Assert
                result.EnsureSuccessStatusCode();
            }
        }
    }
}