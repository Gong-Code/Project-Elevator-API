using AutoFixture;

namespace ElevatorApi.Tests.ControllerTests
{
    public class BaseControllerTest
    {
        protected readonly Fixture Fixture;

        protected BaseControllerTest()
        {
            Fixture = new Fixture();
        }
    }
}
