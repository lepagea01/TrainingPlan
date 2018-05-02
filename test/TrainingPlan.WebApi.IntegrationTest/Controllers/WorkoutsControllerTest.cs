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
            public async Task Returns_Response_With_OkStatusCode()
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